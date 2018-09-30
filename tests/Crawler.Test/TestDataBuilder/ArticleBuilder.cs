using Crawler.Entities;
using System;

namespace Crawler.Test.TestDataBuilder
{
    public class ArticleBuilder
    {
        public static Article CreateOne()
        {
            return new Article(DateTime.UtcNow.Date, "www.google.com", "Google",
                new string[] { "Larry Page", "Sergey Brin" });
        }
    }
}