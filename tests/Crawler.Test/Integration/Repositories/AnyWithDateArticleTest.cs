using System;
using System.Threading.Tasks;
using Crawler.Entities;
using Crawler.Repositories;
using Crawler.SqlServer;
using Crawler.Test.TestDataBuilder;
using FluentAssertions;
using Xunit;

namespace Crawler.Test.Integration.Repositories
{
    [Trait(TraitConstants.Category, TraitConstants.Integration)]
    [Collection("Database collection")]

    public class AnyWithDateArticleTest
    {
        
        readonly IArticles articles;        
    
        public AnyWithDateArticleTest(DatabaseFixture fixture)
        {            
            articles = new Articles(TestHelper.GetConfiguration().SqlServerSettings());
            
        }

        [Fact]
        public async Task ShouldFindAnyWithDate()
        {
            await articles.Add(new Article[] { ArticleBuilder.CreateOne(), ArticleBuilder.CreateOne() });

            var actual = await articles.AnyWithDate(DateTime.UtcNow.Date);
            actual.Should().Be(true);
        }

        [Fact]
        public async Task ShouldNotFindAnyWithDate()
        {
            var actual = await articles.AnyWithDate(new DateTime(2000,1,1));
            actual.Should().Be(false);
        }
    }
}