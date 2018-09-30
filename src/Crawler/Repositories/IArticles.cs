using Crawler.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crawler.Repositories
{
    public interface IArticles
    {
        Task Add(IEnumerable<Article> articles);
        Task<bool> AnyWithDate(DateTime date);
    }
}