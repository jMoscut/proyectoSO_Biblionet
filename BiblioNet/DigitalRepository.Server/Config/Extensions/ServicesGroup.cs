using DigitalRepository.Server.Entities.Models;
using DigitalRepository.Server.Entities.Request;
using DigitalRepository.Server.Interceptors.DocumentInterceptor;
using DigitalRepository.Server.Interceptors.Interfaces;
using DigitalRepository.Server.Services.Core;
using DigitalRepository.Server.Services.Interfaces;
using DigitalRepository.Server.Utils;
using DigitalRepository.Server.Utils.Interfaces;

namespace DigitalRepository.Server.Config.Extensions
{

    /// <summary>
    /// Defines the <see cref="ServicesGroup" />
    /// </summary>
    public static class ServicesGroup
    {
        /// <summary>
        /// The AddServiceGroup
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddServiceGroup(this IServiceCollection services)
        {
            // entities services
            services.AddScoped<IAuthService, AuthService>();
            services
                .AddScoped<IEntityService<Document, DocumentRequest, long>,
                    EntityService<Document, DocumentRequest, long>>();
            //filters
            services.AddScoped<IFilterTranslator<Document>, FilterTranslator<Document>>();
            //interceptors
            services.AddScoped<IEntityBeforeCreateInterceptor<Document, DocumentRequest>, SaveDocumentServer>();
            services.AddScoped<IEntityBeforeCreateInterceptor<Document, DocumentRequest>, ValidatePdfDocument>();

            // other services
            services.AddScoped<ISendMail, SendEmail>();
            services.AddScoped<IResources, Resources>();

            return services;
        }
    }
}
