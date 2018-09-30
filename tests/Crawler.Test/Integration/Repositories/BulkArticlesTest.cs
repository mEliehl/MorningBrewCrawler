using Crawler.Entities;
using Crawler.Repositories;
using Crawler.SqlServer;
using Crawler.Test.TestDataBuilder;
using Dapper;
using FluentAssertions;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;

namespace Crawler.Test.Integration.Repositories
{
    [Trait(TraitConstants.Category, TraitConstants.Integration)]
    [Collection("Database collection")]
    public class BulkArticlesTest
    {
        readonly IArticles articles;
        readonly SqlServerSettings settings;

        public BulkArticlesTest(DatabaseFixture fixture)
        {
            settings = TestHelper.GetConfiguration().SqlServerSettings();
            articles = new Articles(settings);
        }

        [Fact]
        public async Task ShouldSaveAnArticle()
        {
            var article = ArticleBuilder.CreateOne();
            await articles.Add(new Article[]{article,
            ArticleBuilder.CreateOne()});

            var (isAddedArticle, countAddedAuthors) = await WhenAllWrapper(IsArticleAdded(article.Id), CountArticle(article.Id));

            isAddedArticle.Should().Be(true);
            countAddedAuthors.Should().Be(2);
        }

        private static async Task<(T1, T2)> WhenAllWrapper<T1, T2>(Task<T1> task1, Task<T2> task2)
        {
            await Task.WhenAll(task1, task2);
            return (await task1, await task2);
        }

        private async Task<bool> IsArticleAdded(Guid articleId)
        {
            using (var connection = new SqlConnection(settings.ConnectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<bool>("select count(1) from Article where Id=@articleId",
                new
                {
                    articleId
                });
            }
        }

        private async Task<int> CountArticle(Guid articleId)
        {
            using (var connection = new SqlConnection(settings.ConnectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<int>("select count(ArticleId) from Author where ArticleId=@articleId",
                new
                {
                    articleId
                });
            }
        }
    }
}