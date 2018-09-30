using DbUp;
using DocoptNet;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlServer.Migration.Fluent.Migrations;
using System;
using System.IO;

namespace SqlServer.Migration.Fluent
{
    class Program
    {
        static void Main(string[] args)
        {
            MainArgs argument;
            try
            {
                argument = new MainArgs(args);
                var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                var serviceProvider = CreateServices(configuration);
                using (var scope = serviceProvider.CreateScope())
                {
                    EnsureDatabase.For.SqlDatabase(configuration.GetConnectionString("default"));
                    System.Console.WriteLine("criado!!");
                    UpdateDatabase(scope.ServiceProvider, argument);
                }
            }
            catch (DocoptExitException ex)
            {
                Console.WriteLine(ex.Message);

            }
            catch (DocoptInputErrorException ex)
            {
                Console.WriteLine(ex.Message);
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

        private static void UpdateDatabase(IServiceProvider serviceProvider, MainArgs argument)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            if (argument.Rollback.HasValue)
                runner.Rollback(argument.Rollback.Value);

            if (argument.Down.HasValue)
                runner.MigrateDown(argument.Down.Value);

            if (argument.Up.HasValue)
            {
                if (argument.Up > 0)
                    runner.MigrateUp(argument.Up.Value);
                else
                    runner.MigrateUp();
            }
        }
    }
}
