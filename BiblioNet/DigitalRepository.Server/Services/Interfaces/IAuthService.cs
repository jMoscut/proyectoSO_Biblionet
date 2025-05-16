﻿namespace DigitalRepository.Server.Services.Interfaces
{
    using FluentValidation.Results;
    using Entities.Response;
    using Entities.Request;

    /// <summary>
    /// Defines the <see cref="IAuthService" />
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// The Auth
        /// </summary>
        /// <param name="model">The model<see cref="LoginRequest"/></param>
        /// <returns>The <see cref="Response{AuthResponse, List{ValidationFailure}}"/></returns>
        public Response<AuthResponse, List<ValidationFailure>> Auth(LoginRequest model);

        /// <summary>
        /// The ValidateToken
        /// </summary>
        /// <param name="token">The token<see cref="string"/></param>
        /// <returns>The <see cref="Response{string, List{ValidationFailure}}"/></returns>
        public Response<string, List<ValidationFailure>> ValidateToken(string token);

        /// <summary>
        /// The ChangePassword
        /// </summary>
        /// <param name="model">The model<see cref="ChangePasswordRequest"/></param>
        /// <returns>The <see cref="Response{string, List{ValidationFailure}}"/></returns>
        public Response<string, List<ValidationFailure>> ChangePassword(ChangePasswordRequest model);

        /// <summary>
        /// The ResetPassword
        /// </summary>
        /// <param name="model">The model<see cref="ResetPasswordRequest"/></param>
        /// <returns>The <see cref="Response{string, List{ValidationFailure}}"/></returns>
        public Response<string, List<ValidationFailure>> ResetPassword(ResetPasswordRequest model);

        /// <summary>
        /// The RecoveryPassword
        /// </summary>
        /// <param name="model">The model<see cref="RecoveryPasswordRequest"/></param>
        /// <returns>The <see cref="Response{string, List{ValidationFailure}}"/></returns>
        public Response<string, List<ValidationFailure>> RecoveryPassword(RecoveryPasswordRequest model);
    }
}
