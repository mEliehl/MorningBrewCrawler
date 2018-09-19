using System;
using System.Net.Http;
using Crawler.Commands;
using Crawler.HttpClients;
using Crawler.Repositories;
using Crawler.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Runner.HostedServices;

namespace Runner.StartupBlocks
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                });

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);


            services
            .AddHttpClient<IMorningBrewClient, MorningBrewClient>(client =>
            {
                client.BaseAddress = new Uri("http://blog.cwa.me.uk/");

            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy); // We place the timeoutPolicy inside the retryPolicy, to make it time out each try.

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

        public static IServiceCollection AddSettings(this IServiceCollection services
            , IConfiguration configuration)
        {
            var triggerOptions = configuration.GetSection("TriggerOptions").Get<TriggerOptions>();
            services.AddSingleton(triggerOptions);

            return services;
        }
    }
}