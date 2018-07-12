using System.Collections.Generic;
using System.Threading.Tasks;
using Crawler.Entities;
using Crawler.HttpFactories;
using HtmlAgilityPack;
using System.Linq;
using System;
using System.Web;
using Crawler.Mappers;
using Crawler.Repositories;

namespace Crawler.Commands
{
    public class CrawlerCommand : ICommand
    {
        public CrawlerCommand(int limit)
        {
            Limit = limit;
        }
        public int Limit { get;}
    }

    public class CrawlerCommandHandler : ICommandHandler<CrawlerCommand>
    {
        readonly IHttpHandler httpHandler;
        readonly IArticles articles; 

        public CrawlerCommandHandler(IHttpHandler httpHandler,
            IArticles articles)
        {
            this.httpHandler = httpHandler;
            this.articles = articles;
        }

        public async Task HandleAsync(CrawlerCommand command)
        {
            var entities = new List<Article>();
            for (int i = 1; i <= command.Limit; i++)
            {
                var page = await httpHandler.HandleAsync(i);
                page = HttpUtility.HtmlDecode(page);
                
                var extractedArticles = Extract(page);

                var groupedArticles = extractedArticles.OrderBy(o => o.Date).GroupBy(g => g.Date);
                foreach(var group in groupedArticles)
                {
                    var postDate = group.FirstOrDefault().Date;
                    if(await articles.AnyWithDate(postDate))
                        break;
                    entities.AddRange(group);
                }
            }
            await articles.Add(entities);
        }

        private IEnumerable<Article> Extract(string page)
        {
            var @return = new List<Article>();
            var doc = new HtmlDocument();
            doc.LoadHtml(page);
            List<HtmlNode> posts = ExtractPosts(doc);

            foreach (var post in posts)
            {
                var postDate = ExtractPostDate(post);
                var date = ExtractPostDate(postDate);

                List<HtmlNode> links = ExtractLinks(post);
                foreach (var link in links)
                {
                    IEnumerable<Article> articles = ExtractArticles(link, date);                    
                    @return.AddRange(articles);
                }
            }

            return @return;
        }

        private DateTime ExtractPostDate(string postDate)
        {
            var split = postDate.Split(' ');
            if(split.Count() != 4)
                throw new ArgumentException($"{postDate} is invalid post date");
            var day = split[1];
            day = day.Substring(0,day.Length - 2);
            var month = EnglishMonths.Map(split[2]);
            var year = split[3];
            return new DateTime(int.Parse(year),month,int.Parse(day));
        }

        private string ExtractPostDate(HtmlNode post)
        {
            var p = post.Descendants("p").FirstOrDefault();
            return HttpUtility.HtmlDecode(p.SelectNodes("em")[1].InnerText);
        }

        private IEnumerable<Article> ExtractArticles(HtmlNode link, DateTime date)
        {
            var tags = link.Descendants("a").ToList();
            var authors = NormalizeAuthor(link.InnerText);

            var articles = tags.Select(a =>
                new Article(date,
                a.Attributes["href"].Value,
                HttpUtility.HtmlDecode(a.InnerHtml),
                authors));
            return articles;
        }

        private List<HtmlNode> ExtractLinks(HtmlNode post)
        {
            return post.Descendants("li").ToList();
        }            

        private List<HtmlNode> ExtractPosts(HtmlDocument doc)
        {
            return doc.DocumentNode
                            .SelectNodes("//div[@class='post']")
                            .ToList();
        }

        private string NormalizeAuthor(string text)
        {
            text = text.Trim();
            var delimeter = " – ";
            text = text.Substring(text.LastIndexOf(delimeter) + delimeter.Length);
            var authors = text.Trim();

            return authors;
        }
    }
}