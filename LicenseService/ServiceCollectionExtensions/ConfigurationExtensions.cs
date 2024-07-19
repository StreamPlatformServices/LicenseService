using LicenseService.Configuration;

namespace APIGatewayMain.ServiceCollectionExtensions
{
    internal static class ConfigurationExtensions
    {
        public static IServiceCollection AddCommonConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeyServiceApiSettings>(configuration.GetSection("KeyServiceApiSettings"));
            
            return services;
        }
    }
}
