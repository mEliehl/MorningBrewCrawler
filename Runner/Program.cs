using System;
using System.Threading.Tasks;
using Crawler.Commands;
using Crawler.HttpFactories;
using Crawler.Repositories;
using Crawler.SqlServer;

namespace Runner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");

            IHttpHandler httpHandler = new MorningbrewHttpHandler();
            IArticles articles = new Articles();
            var commandHandler = new CrawlerCommandHandler(httpHandler,articles);
            var command = new CrawlerCommand(1,50);
            await commandHandler.HandlerAsync(command);


            System.Console.WriteLine("Finished");
        }
    }
}
