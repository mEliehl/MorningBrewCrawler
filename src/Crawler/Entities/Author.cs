using System;
using System.Collections.Generic;

namespace Crawler.Entities
{
    public class Author
    {
        public Author(Guid articleId, string name)
        {
            ArticleId = articleId;
            Name = name;
        }

        public Guid ArticleId { get; private set; }

        public string Name { get; private set; }
    }
}