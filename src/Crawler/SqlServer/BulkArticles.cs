using Crawler.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.SqlServer
{
    internal class BulkArticles
    {
        public static async Task Add(IEnumerable<Article> articles, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var transcation = connection.BeginTransaction())
                {
                    var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transcation)
                    {
                        DestinationTableName = "Article"
                    };
                    var dtArticles = MakeArticleTable(articles);
                    await bulk.WriteToServerAsync(dtArticles);

                    bulk.DestinationTableName = "Author";
                    var dtAuthors = MakeAuthorTable(articles.SelectMany(s => s.Authors));
                    await bulk.WriteToServerAsync(dtAuthors);

                    transcation.Commit();
                }
            }
        }

        private static DataTable MakeArticleTable(IEnumerable<Article> articles)
        {
            var dt = new DataTable("Article");
            dt.Columns.Add("Id", typeof(Guid));
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            foreach (var article in articles)
                dt.Rows.Add(MakeArticleRow(article));

            return dt;
        }

        private static object[] MakeArticleRow(Article article) => new object[]{
                article.Id,
                article.Date,
                article.Link,
                article.Title
            };

        private static DataTable MakeAuthorTable(IEnumerable<Author> authors)
        {
            var dt = new DataTable("Author");
            dt.Columns.Add("ArticleId", typeof(Guid));
            dt.Columns.Add("Name", typeof(string));
            foreach (var author in authors)
                dt.Rows.Add(MakeAuthorRow(author));

            return dt;
        }

        private static object[] MakeAuthorRow(Author author) => new object[]{
                author.ArticleId,
                author.Name
            };
    }
}