using DigitalRepository.Server.Mappers;
using Mapster;
using MapsterMapper;

namespace DigitalRepository.Server.Config.Extensions
{
    public static class MapsterSettings
    {
        public static IServiceCollection AddMapsterSettings(this IServiceCollection services)
        {

            MapsterConfig.RegisterMappings();
            services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
