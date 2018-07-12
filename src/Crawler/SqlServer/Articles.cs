using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Crawler.Entities;
using Crawler.Repositories;
using Dapper;

namespace Crawler.SqlServer
{
    public class Articles : IArticles
    {
        readonly SqlServerSettings settings;

        public Articles(SqlServerSettings settings)
        {
            this.settings = settings;
        }

        public async Task Add(IEnumerable<Article> articles)
        {
            using (var connection = new SqlConnection(settings.ConnectionString))
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

        private DataTable MakeArticleTable(IEnumerable<Article> articles)
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

        private object[] MakeArticleRow(Article article)
        {
            return new object[]{
                article.Id,
                article.Date,
                article.Link,
                article.Title,
                article.Authors
            };
        }

        public async Task<bool> AnyWithDate(DateTime date)
        {
            using (var connection = new SqlConnection(settings.ConnectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<bool>("select count(1) from Article where date=@date",
                new
                {
                    date
                });
            }
        }
    }
}