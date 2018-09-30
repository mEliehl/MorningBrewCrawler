using Crawler.SqlServer;
using Dapper;
using System;
using System.Data.SqlClient;

namespace Crawler.Test.Integration
{
    public class DatabaseFixture : IDisposable
    {
        readonly SqlServerSettings settings;
        public DatabaseFixture()
        {
            settings = TestHelper.GetConfiguration().SqlServerSettings();
        }
        public void Dispose()
        {
            using (var connection = new SqlConnection(settings.ConnectionString))
            {
                connection.Execute("delete from Article");
            }
        }
    }
}