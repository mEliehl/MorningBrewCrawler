using Crawler.Commands;
using Crawler.HttpFactories;
using Crawler.Repositories;
using Crawler.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Runner.StartupBlocks
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddSingleton<IHttpHandler, MorningbrewHttpHandler>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("default");
            var settings = new SqlServerSettings(connectionString);
            services.AddSingleton(settings);
            services.AddSingleton<IArticles, Articles>();

            return services;
        }

        public static IServiceCollection AddCommanHandlers(this IServiceCollection services)
        {
            services.AddSingleton<ICommandHandler<CrawlerCommand>, CrawlerCommandHandler>();
            return services;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<TriggerCommandService>();
            return services;
        }
    }
}