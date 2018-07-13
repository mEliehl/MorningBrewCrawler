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
            Console.WriteLine($"Starting runner at:{DateTime.Now}");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine($"Starting crawler at:{DateTime.Now}");
            crawlerHandler.HandleAsync(new CrawlerCommand(50)).Wait();
            Console.WriteLine($"Finished crawler at:{DateTime.Now}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Stopped runner at:{DateTime.Now}");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}