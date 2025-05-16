﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DigitalRepository.Server.Config.Entities;
using DigitalRepository.Server.Context;
using DigitalRepository.Server.Entities.Models;
using DigitalRepository.Server.Entities.Request;
using DigitalRepository.Server.Entities.Response;
using DigitalRepository.Server.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace DigitalRepository.Server.Services.Core
{
    /// <summary>
    /// Defines the <see cref="AuthService" />
    /// </summary>
    [AllArgsConstructor]
    public partial class AuthService : IAuthService
    {
        /// <summary>
        /// Defines the _bd
        /// </summary>
        private readonly DataContext _bd;

        /// <summary>
        /// Defines the _appSettings
        /// </summary>
        private readonly IOptions<AppSettings> _appSettings;

        /// <summary>
        /// Defines the _loginValidations
        /// </summary>
        private readonly IValidator<LoginRequest> _loginValidations;

        /// <summary>
        /// Defines the _changePasswordValidations
        /// </summary>
        private readonly IValidator<ChangePasswordRequest> _changePasswordValidations;

        /// <summary>
        /// Defines the _resetPasswordValidator
        /// </summary>
        private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;

        /// <summary>
        /// Defines the _recoveryPasswordValidator
        /// </summary>
        private readonly IValidator<RecoveryPasswordRequest> _recoveryPasswordValidator;

        /// <summary>
        /// Defines the _sendMail
        /// </summary>
        private readonly ISendMail _sendMail;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// The Auth
        /// </summary>
        /// <param name="model">The model<see cref="LoginRequest"/></param>
        /// <returns>The <see>
        ///         <cref>Response{AuthResponse, List{ValidationFailure}}</cref>
        ///     </see>
        /// </returns>
        public Response<AuthResponse, List<ValidationFailure>> Auth(LoginRequest model)
        {
            Response<AuthResponse, List<ValidationFailure>> userResponse = new();
            try
            {
                ValidationResult results = _loginValidations.Validate(model);

                if (!results.IsValid)
                {
                    userResponse.Success = false;
                    userResponse.Message = "Usuario y/o contraseña invalidos";
                    userResponse.Data = null;
                    userResponse.Errors = results.Errors;

                    return userResponse;
                }

                User? oUser = _bd.Users.Include(user => user.Rol!).FirstOrDefault(u =>
                    u.Email == model.UserName);

                if (oUser == null)
                {
                    userResponse.Success = false;
                    userResponse.Message = "Usuario y/o contraseña invalidos";
                    userResponse.Data = null;
                    userResponse.Errors = results.Errors;

                    return userResponse;
                }

                if (!BC.BCrypt.Verify(model.Password, oUser.Password))
                {
                    userResponse.Success = false;
                    userResponse.Message = "Usuario y/o contraseña invalidos";
                    userResponse.Data = null;
                    userResponse.Errors = results.Errors;

                    return userResponse;
                }

                List<RolOperation> rolOperations = _bd.RolOperations.Include(r => r.Operation)
                    .Where(r => r.RolId == oUser.RolId && r.State == 1).ToList();

                List<Operation> operationsRol = rolOperations.Select(ro => new Operation()
                {
                    Name = ro.Operation!.Name,
                    ModuleId = ro.Operation.ModuleId,
                    Id = ro.Operation.Id,
                    CreatedAt = ro.Operation.CreatedAt,
                    IsVisible = ro.Operation.IsVisible,
                    Icon = ro.Operation.Icon,
                    Path = ro.Operation.Path,
                    Description = ro.Operation.Description,
                    CreatedBy = ro.Operation.CreatedBy, 
                    UpdatedAt = ro.Operation.UpdatedAt,
                    UpdatedBy = ro.Operation.UpdatedBy,
                    Guid = ro.Operation.Guid,
                    Policy = ro.Operation.Policy,
                    State = ro.Operation.State,
                }).ToList();

                var modules = _bd.Modules
                    .FromSqlInterpolated($"""
                                              SELECT 
                                                  mo."Id", 
                                                  mo."Name", 
                                                  mo."Description", 
                                                  mo."Image", 
                                                  mo."Path", 
                                                  mo."State", 
                                                  mo."CreatedAt", 
                                                  mo."UpdatedAt", 
                                                  mo."CreatedBy", 
                                                  mo."UpdatedBy"
                                              FROM (
                                                  SELECT o."ModuleId"
                                                  FROM "RolOperations" ro
                                                  INNER JOIN "Operations" o ON o."Id" = ro."OperationId"
                                                  INNER JOIN "Modules" m ON o."ModuleId" = m."Id"
                                                  WHERE ro."RolId" = {oUser.RolId} AND ro."State" = 1
                                                  GROUP BY o."ModuleId"
                                              ) AS mod
                                              INNER JOIN "Modules" mo ON mod."ModuleId" = mo."Id"
                                          """)
                    .ToList();

                List<Authorizations> authorizations = modules
                    .Select(module => new Authorizations() { Module = module, Operations = operationsRol.Where(o => o.ModuleId == module.Id).ToList() })
                    .ToList();

                oUser.Rol!.RolOperations = rolOperations;

                string jwt = GetToken(oUser);

                AuthResponse auth = new()
                {
                    UserId = oUser.Id,
                    Name = oUser.Name,
                    UserName = "",
                    Email = oUser.Email,
                    Token = jwt,
                    Redirect = oUser.Reset!.Value,
                    Rol = oUser.RolId,
                    Operations = authorizations
                };

                userResponse.Success = true;
                userResponse.Message = "Inicio de sesión exitosa";
                userResponse.Data = auth;
                userResponse.Errors = null;

                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.Success = false;
                userResponse.Message = "Upss hubo un error";
                userResponse.Data = null;
                userResponse.Errors = [new("Exception", ex.Message)];

                _logger.LogError(ex, "Error al autenticar usuario path: /api/Auth");

                return userResponse;
            }
        }

        /// <summary>
        /// The ChangePassword
        /// </summary>
        /// <param name="model">The model<see cref="ChangePasswordRequest"/></param>
        /// <returns>The <see>
        ///         <cref>Response{string, List{ValidationFailure}}</cref>
        ///     </see>
        /// </returns>
        public Response<string, List<ValidationFailure>> ChangePassword(ChangePasswordRequest model)
        {

            Response<string, List<ValidationFailure>> response = new();

            try
            {
                ValidationResult results = _changePasswordValidations.Validate(model);

                if (!results.IsValid)
                {
                    response.Success = false;
                    response.Message = "Error al hacer la solicitud";
                    response.Data = "";
                    response.Errors = results.Errors;

                    return response;
                }

                User? oUser = _bd.Users.FirstOrDefault(u => u.RecoveryToken == model.Token);

                if (oUser == null)
                {
                    response.Success = false;
                    response.Message = "El token no es valido";
                    response.Data = "";
                    response.Errors = results.Errors;
                    return response;
                }

                if (model.Password != model.ConfirmPassword)
                {
                    response.Success = false;
                    response.Message = "Las Contraseñas no coinciden";
                    response.Data = "";
                    response.Errors = results.Errors;
                    return response;
                }

                if (BC.BCrypt.Verify(model.Password, oUser.Password))
                {
                    response.Success = false;
                    response.Message = "La nueva contraseña debe ser distinta a la contraseña anterior";
                    response.Data = "";
                    response.Errors = results.Errors;
                    return response;
                }

                string encrypt = BC.BCrypt.HashPassword(model.Password);

                oUser.Password = encrypt;
                oUser.RecoveryToken = "";
                oUser.Reset = false;
                oUser.UpdatedAt = DateTime.Now;
                oUser.UpdatedBy = oUser.Id;

                _bd.Users.Update(oUser);
                _bd.SaveChanges();

                response.Success = true;
                response.Message = "Cambio de Contraseña Exitoso";
                response.Data = $"tu nueva contraseña es: {model.Password}";
                response.Errors = results.Errors;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al hacer la solicitud {ex.Message}";
                response.Data = "";
                response.Errors = [new("Exception", ex.Message)];

                _logger.LogError(ex, "Error al cambiar la contraseña path: /api/Auth/ChangePassword");

                return response;
            }
        }

        /// <summary>
        /// The ResetPassword
        /// </summary>
        /// <param name="model">The model<see cref="ResetPasswordRequest"/></param>
        /// <returns>The <see>
        ///         <cref>Response{string, List{ValidationFailure}}</cref>
        ///     </see>
        /// </returns>
        public Response<string, List<ValidationFailure>> ResetPassword(ResetPasswordRequest model)
        {

            Response<string, List<ValidationFailure>> response = new();

            try
            {
                ValidationResult results = _resetPasswordValidator.Validate(model);

                if (!results.IsValid)
                {
                    response.Success = false;
                    response.Message = "Error al hacer la solicitud";
                    response.Data = "";
                    response.Errors = results.Errors;

                    return response;
                }

                string encrypt = BC.BCrypt.HashPassword(model.Password);

                User? oUser = _bd.Users.FirstOrDefault(u => u.Id == model.IdUser);

                if (oUser == null)
                {
                    response.Success = false;
                    response.Message = "Usuario no encontrado";
                    response.Data = "";
                    response.Errors = results.Errors;
                    return response;
                }

                oUser.Password = encrypt;
                oUser.RecoveryToken = "";
                oUser.Reset = false;
                oUser.UpdatedAt = DateTime.Now;
                oUser.UpdatedBy = oUser.Id;

                _bd.Users.Update(oUser);
                _bd.SaveChanges();

                response.Success = true;
                response.Message = "Cambio de Contraseña Exitoso";
                response.Data = $"tu nueva contraseña es: {model.Password}";
                response.Errors = results.Errors;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al hacer la solicitud {ex.Message}";
                response.Data = "";
                response.Errors = [new("Exception", ex.Message)];

                _logger.LogError(ex, "Error al reestablecer la contraseña path: /api/Auth/ResetPassword");
            }

            return response;
        }

        /// <summary>
        /// The ValidateToken
        /// </summary>
        /// <param name="token">The token<see cref="string"/></param>
        /// <returns>The <see>
        ///         <cref>Response{string, List{ValidationFailure}}</cref>
        ///     </see>
        /// </returns>
        public Response<string, List<ValidationFailure>> ValidateToken(string token)
        {
            Response<string, List<ValidationFailure>> response = new();

            try
            {
                string decodedToken = Uri.UnescapeDataString(token);

                User? oUser = _bd.Users.FirstOrDefault(u => u.RecoveryToken == decodedToken);

                if (oUser == null)
                {
                    response.Success = false;
                    response.Message = "Su Token ya ha Expirado";
                    response.Data = token;
                    response.Errors = [];

                    return response;
                }

                var currentDate = DateTime.Now;

                if (currentDate.CompareTo(oUser.DateToken?.AddMinutes(15)) >= 0)
                {
                    response.Success = false;
                    response.Message = "Tu Token ya ha Expirado";
                    response.Data = token;
                    response.Errors = [];

                    return response;
                }

                response.Success = true;
                response.Message = "Token Válido";
                response.Data = token;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error al verificar token";
                response.Data = token;
                response.Errors = [new ValidationFailure("Exception", ex.Message)];

                _logger.LogError(ex, "Error al verificar token path: api/Auth/Token/[token]");

                return response;
            }
        }

        /// <summary>
        /// The RecoveryPassword
        /// </summary>
        /// <param name="model">The model<see cref="RecoveryPasswordRequest"/></param>
        /// <returns>The <see>
        ///         <cref>Response{string, List{ValidationFailure}}</cref>
        ///     </see>
        /// </returns>
        public Response<string, List<ValidationFailure>> RecoveryPassword(RecoveryPasswordRequest model)
        {
            Response<string, List<ValidationFailure>> response = new();

            try
            {
                ValidationResult results = _recoveryPasswordValidator.Validate(model);

                if (!results.IsValid)
                {
                    response.Success = false;
                    response.Message = "Error al hacer la solicitud";
                    response.Data = "";
                    response.Errors = results.Errors;

                    return response;
                }

                User? oUser = _bd.Users.FirstOrDefault(u => u.Email == model.Email);

                if (oUser == null)
                {
                    response.Success = false;
                    response.Message = "Usuario no encontrado";
                    response.Data = "";
                    response.Errors = results.Errors;

                    return response;
                }

                string token = BC.BCrypt.HashPassword(Guid.NewGuid().ToString());

                oUser.RecoveryToken = token;
                oUser.Reset = true;
                oUser.DateToken = DateTime.Now;
                oUser.UpdatedAt = DateTime.Now;
                oUser.UpdatedBy = oUser.Id;

                _bd.Users.Update(oUser);
                _bd.SaveChanges();

                string bodyMail = $"Hola {oUser.Name} <br> Este es su token para reestablecer contraseña <br> {token}";

                if (!_sendMail.Send(oUser.Email, "Recuperar Contraseña", bodyMail))
                {
                    response.Success = false;
                    response.Message = "Error al enviar el correo porfavor verifique";
                    response.Data = "";

                    return response;
                }

                response.Success = true;
                response.Message = "Correo Enviado Con Exito";
                response.Errors = results.Errors;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al hacer la solicitud {ex.Message}";
                response.Data = "";
                response.Errors = [new("Exception", ex.Message)];

                _logger.LogError(ex, "Error al recuperar contraseña path: /api/Auth/RecoveryPassword");

                return response;
            }
        }

        /// <summary>
        /// The GetToken
        /// </summary>
        /// <param name="user">The user<see cref="User"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string GetToken(User user)
        {
            try
            {
                AppSettings appSettings = _appSettings.Value;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var claims = new List<Claim>()
                             {
                                 new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                                 new (ClaimTypes.Email, user.Email),
                                 new (ClaimTypes.Name, user.Name),
                                 new (ClaimTypes.Hash, Guid.NewGuid().ToString()),
                                 new ("Operator", user.RolId.ToString()),
                             };

                if (user.Rol!.RolOperations.Count != 0)
                {
                    claims.AddRange(user.Rol!.RolOperations.Select(item => new Claim(ClaimTypes.Role, item.OperationId.ToString())));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims.ToArray()),
                    NotBefore = DateTime.UtcNow.AddMinutes(appSettings.NotBefore),
                    Expires = DateTime.UtcNow.AddHours(appSettings.TokenExpirationHrs),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error al Generar el jwt");

                return string.Empty;
            }
        }
    }
}
