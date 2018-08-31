using System;
using System.Collections.Generic;
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
            await BulkArticles.Add(articles,settings.ConnectionString);
        }

        public async Task<bool> AnyWithDate(DateTime date)
        {
            using (var connection = new SqlConnection(settings.ConnectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<bool>("select 1 from Article where date=@date",
                new
                {
                    date
                });
            }
        }
    }
}