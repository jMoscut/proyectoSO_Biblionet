using DigitalRepository.Server.Entities.Request;
using DigitalRepository.Server.Validations.Auth;
using DigitalRepository.Server.Validations.Documents;

namespace DigitalRepository.Server.Config.Extensions
{
    using FluentValidation;

    /// <summary>
    /// Defines the <see cref="ValidationsGroup" />
    /// </summary>
    public static class ValidationsGroup
    {
        /// <summary>
        /// The AddValidationsGroup
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddValidationsGroup(this IServiceCollection services)
        {
            //auth validations
            services.AddScoped<IValidator<LoginRequest>, LoginValidations>();
            //login validations
            services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordValidations>();
            services.AddScoped<IValidator<ResetPasswordRequest>, ResetPasswordValidations>();
            services.AddScoped<IValidator<RecoveryPasswordRequest>, RecoveryPasswordValidations>();
            //document validations
            services.AddKeyedScoped<IValidator<DocumentRequest>, DocumentCreateValidator>("Create");
            services.AddKeyedScoped<IValidator<DocumentRequest>, DocumentUpdateValidator>("Update");
            services.AddKeyedScoped<IValidator<DocumentRequest>, DocumentPartialUpdateValidator>("Partial");

            return services;
        }
    }

}
