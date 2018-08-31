using Xunit;

namespace Crawler.Test.Integration
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        
    }
}