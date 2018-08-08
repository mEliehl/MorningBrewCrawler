using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Entities
{
    public class Article
    {
        public Article(DateTime date, string link, string title, IEnumerable<string> authors)
        {
            Id = Guid.NewGuid();
            Date = date;
            Link = link;
            Title = title;
            Authors = authors.Select(a => new Author(Id,a));
        }

        public Guid Id { get; private set; }

        public DateTime Date { get; private set; }

        public string Link { get; private set; }

        public string Title { get; private set; }

        public IEnumerable<Author> Authors { get; private set; }
    }
}