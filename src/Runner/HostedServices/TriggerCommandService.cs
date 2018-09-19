using System;
using System.Threading;
using System.Threading.Tasks;
using Crawler.Commands;
using Microsoft.Extensions.Hosting;

namespace Runner.HostedServices
{
    public class TriggerCommandService : IHostedService, IDisposable
    {
        readonly ICommandHandler<CrawlerCommand> crawlerHandler;
        readonly TriggerOptions options;
        private Timer _timer;

        public TriggerCommandService(ICommandHandler<CrawlerCommand> crawlerHandler,
            TriggerOptions options)
        {
            this.crawlerHandler = crawlerHandler;
            this.options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Starting runner in Utc: {DateTime.UtcNow}");
            
            Console.WriteLine($"Crawler will be executed in UTC: {options.GetScheduledTime()}");

            var untilScheduled = new TimeSpan((options.GetScheduledTime() - DateTime.UtcNow).Ticks);
            _timer = new Timer(DoWork, null, dueTime: untilScheduled,
                period: TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine($"Starting crawler in Utc: {DateTime.UtcNow}");

            crawlerHandler.HandleAsync(new CrawlerCommand(options.PageLimit)).Wait();

            Console.WriteLine($"Finished crawler in Utc: {DateTime.UtcNow}");

            Console.WriteLine($"Crawler will be executed in UTC: {options.GetScheduledTime()}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Stopped runner at Utc:{DateTime.UtcNow}");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}