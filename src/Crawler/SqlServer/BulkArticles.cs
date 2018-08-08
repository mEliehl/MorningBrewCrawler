using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Crawler.Entities;

namespace Crawler.SqlServer
{
    internal class BulkArticles
    {
        public static async Task Add(IEnumerable<Article> articles, string connectionString )
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlTransaction transcation = connection.BeginTransaction())
                {
                    SqlBulkCopy bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transcation);

                    bulk.DestinationTableName = "Article";

                    var dt = MakeArticleTable(articles);
                    await bulk.WriteToServerAsync(dt);

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
            dt.Columns.Add("Authors", typeof(string));
            foreach (var article in articles)
            {
                dt.Rows.Add(MakeArticleRow(article));
            }
            return dt;
        }

        private static object[] MakeArticleRow(Article article)
        {
            return new object[]{
                article.Id,
                article.Date,
                article.Link,
                article.Title,
                article.Authors
            };
        }
    }
}