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

        public static WebApplicationBuilder AddKestrelSettings(this WebApplicationBuilder builder, KestrelSettings kestrelSettings)
        {
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Listen(System.Net.IPAddress.Parse(kestrelSettings.ListeningIPv4Address), kestrelSettings.PortNumber);

                if (kestrelSettings.UseTls)
                {
                    serverOptions.Listen(System.Net.IPAddress.Parse(kestrelSettings.ListeningIPv4Address), kestrelSettings.TlsPortNumber, listenOptions =>
                    {
                        listenOptions.UseHttps();
                    });
                }
            });

            return builder;
        }
    }
}
