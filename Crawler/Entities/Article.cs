using System;

namespace Crawler.Entities
{
    public class Article
    {
        public Article(DateTime date, string link, string title, string authors)
        {
            Id = Guid.NewGuid();
            Date = date;
            Link = link;
            Title = title;
            Authors = authors;
        }

        public Guid Id { get; private set; }

        public DateTime Date { get; private set; }

        public string Link { get; private set; }

        public string Title { get; private set; }

        public string Authors { get; private set; }
    }
}