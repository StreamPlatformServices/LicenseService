using KeyServiceAPI;
using LicenseService.Configuration;
using Microsoft.Extensions.Options;

namespace APIGatewayMain.ServiceCollectionExtensions.ComponentsExtensions
{
    public static class KeyServiceApiExtensions
    {
        public static IServiceCollection AddKeyServiceAPI(this IServiceCollection services)
        {
            services.AddHttpClient<IKeyServiceClient, KeyServiceClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<KeyServiceApiSettings>>().Value;
                client.BaseAddress = new Uri(options.KeyServiceUrl);
            });

            return services;
        }
    }

}
