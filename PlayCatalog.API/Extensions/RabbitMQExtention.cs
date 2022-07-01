using MassTransit;
using PlayCatalog.API.Settings;
using ServiceSettings = PlayCatalog.Application.Settings.ServiceSettings;

namespace PlayCatalog.API.Extensions
{
    public  static class RabbitMqExtention
    {
        public static IServiceCollection AddRabbitMQConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            var _settings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            services.AddMassTransit(x =>
            {
                
                x.UsingRabbitMq((context, config) =>
                {
                    var rabbitMqSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    config.Host(rabbitMqSettings.Host);
                    config.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(_settings.ServiceName, false));
                });
            });
            services.AddSingleton<IHostedService, MassTransitHostedService>();
            return services;
        }

    }
}
