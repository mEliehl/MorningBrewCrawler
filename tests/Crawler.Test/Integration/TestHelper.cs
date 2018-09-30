using Crawler.SqlServer;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Crawler.Test.Integration
{
    public static class TestHelper
    {
        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        public static SqlServerSettings SqlServerSettings(this IConfigurationRoot configuration)
        {
            var connectionString = configuration.GetConnectionString("default");
            var settings = new SqlServerSettings(connectionString);
            return settings;
        }

    }
}