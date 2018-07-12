using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crawler.Entities;

namespace Crawler.Repositories
{
    public interface IArticles
    {
        Task Add(IEnumerable<Article> articles);
        Task<bool> AnyWithDate(DateTime date);
    }
}