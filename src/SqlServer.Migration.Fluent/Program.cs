using System;
using System.IO;
using DbUp;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlServer.Migration.Fluent.Migrations;

namespace SqlServer.Migration.Fluent
{
    class Program
    {
        static void Main(string[] args)
        {            
            foreach(var arg in args)
                Console.WriteLine($"args={arg}");                            
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var serviceProvider = CreateServices(configuration);

            using (var scope = serviceProvider.CreateScope())
            {
                EnsureDatabase.For.SqlDatabase(configuration.GetConnectionString("default"));
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        private static IServiceProvider CreateServices(IConfigurationRoot configuration)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(configuration.GetConnectionString("default"))
                    .ScanIn(typeof(CreateArticleTable).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            
            runner.MigrateUp();
        }
    }
}
