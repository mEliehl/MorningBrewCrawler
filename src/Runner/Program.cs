using System;
using System.IO;
using System.Threading.Tasks;
using Crawler.Commands;
using Crawler.HttpFactories;
using Crawler.Repositories;
using Crawler.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Runner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // var host = new HostBuilder()
            //     .ConfigureHostConfiguration(configHost =>
            //     {
            //         configHost.SetBasePath(Directory.GetCurrentDirectory());
            //         configHost.AddJsonFile("hostsettings.json", optional: true);
            //         configHost.AddEnvironmentVariables(prefix: "PREFIX_");
            //         configHost.AddCommandLine(args);
            //     })
            //     .ConfigureAppConfiguration((hostContext, configApp) =>
            //     {
            //         configApp.AddJsonFile("appsettings.json", optional: true);
            //         configApp.AddJsonFile(
            //             $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
            //             optional: true);
            //         configApp.AddEnvironmentVariables(prefix: "PREFIX_");
            //         configApp.AddCommandLine(args);
            //     })
            //     .ConfigureServices((hostContext, services) =>
            //     {
            //         services.AddLogging();
            //     })
            //     .ConfigureLogging((hostContext, configLogging) =>
            //     {
            //         configLogging.AddConsole();
            //         configLogging.AddDebug();
            //     })
            //     .UseConsoleLifetime()
            //     .Build();

            // await host.RunAsync();

            Console.WriteLine("Starting...");

            IHttpHandler httpHandler = new MorningbrewHttpHandler();
            var settings = new SqlServerSettings("Data Source=localhost;Initial Catalog=MorningBrew;Persist Security Info=True;User ID=sa;Password=Crawler_Brew");
            IArticles articles = new Articles(settings);
            var commandHandler = new CrawlerCommandHandler(httpHandler,articles);
            var command = new CrawlerCommand(50);
            await commandHandler.HandleAsync(command);


            System.Console.WriteLine("Finished");
        }
    }
}
