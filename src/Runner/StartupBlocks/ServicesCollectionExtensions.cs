using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Runner.HostedServices;

namespace Runner.StartupBlocks
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<TriggerCommandService>();
            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services
            , IConfiguration configuration)
        {
            var triggerOptions = configuration.GetSection("TriggerOptions").Get<TriggerOptions>();
            services.AddSingleton(triggerOptions);

            return services;
        }
    }
}