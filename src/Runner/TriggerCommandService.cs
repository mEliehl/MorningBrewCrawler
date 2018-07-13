using System;
using System.Threading;
using System.Threading.Tasks;
using Crawler.Commands;
using Microsoft.Extensions.Hosting;

namespace Runner
{
    public class TriggerCommandService : IHostedService, IDisposable
    {
        readonly ICommandHandler<CrawlerCommand> crawlerHandler;
        private Timer _timer;

        public TriggerCommandService(ICommandHandler<CrawlerCommand> crawlerHandler)
        {
            this.crawlerHandler = crawlerHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Starting runner at Utc:{DateTime.UtcNow}");

            _timer = new Timer(DoWork, null, TimeSpan.FromMinutes(0), 
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine($"Starting crawler at Utc:{DateTime.UtcNow}");
            crawlerHandler.HandleAsync(new CrawlerCommand(50)).Wait();
            Console.WriteLine($"Finished crawler at Utc:{DateTime.UtcNow}");
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