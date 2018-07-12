namespace Crawler.SqlServer
{
    public class SqlServerSettings
    {
        public SqlServerSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}