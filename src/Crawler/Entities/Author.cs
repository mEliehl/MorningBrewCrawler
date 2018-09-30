using System;

namespace Crawler.Entities
{
    public class Author
    {
        internal Author(Guid articleId, string name)
        {
            ArticleId = articleId;
            Name = name;
        }

        public Guid ArticleId { get; private set; }

        public string Name { get; private set; }
    }
}