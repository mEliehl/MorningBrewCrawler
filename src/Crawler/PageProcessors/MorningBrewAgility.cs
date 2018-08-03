using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crawler.Entities;
using Crawler.Mappers;
using HtmlAgilityPack;

namespace Crawler.PageProcessors
{
    public class MorningBrewAgility : IMorningBrewProcessor
    {
        public IEnumerable<Article> Map(string page)
        {
            page = HttpUtility.HtmlDecode(page);
            
            var @return = new List<Article>();
            var doc = new HtmlDocument();
            doc.LoadHtml(page);
            var posts = ExtractPosts(doc);

            foreach (var post in posts)
            {
                var date = ExtractPostDate(post);

                var links = ExtractLinks(post);
                foreach (var link in links)
                {
                    var articles = ExtractArticles(link, date);
                    @return.AddRange(articles);
                }
            }

            return @return;
        }

        private IEnumerable<HtmlNode> ExtractPosts(HtmlDocument doc)
        {
            return doc.DocumentNode
                .SelectNodes("//div[@class='post']")
                .ToList();
        }

        private DateTime ExtractPostDate(string postDate)
        {
            var split = postDate.Split(' ');
            if (split.Count() != 4)
                throw new ArgumentException($"{postDate} is invalid post date");
            var day = split[1];
            day = day.Substring(0, day.Length - 2);
            var month = EnglishMonths.Map(split[2]);
            var year = split[3];
            return new DateTime(int.Parse(year), month, int.Parse(day));
        }

        private DateTime ExtractPostDate(HtmlNode post)
        {
            var node = post.Descendants("p").FirstOrDefault();
            var postDate = HttpUtility.HtmlDecode(node.SelectNodes("em")[1].InnerText);
            return ExtractPostDate(postDate);
        }
        private IEnumerable<HtmlNode> ExtractLinks(HtmlNode post)
        {
            return post.Descendants("li").ToList();
        }

        private IEnumerable<Article> ExtractArticles(HtmlNode link, DateTime date)
        {
            var tags = link.Descendants("a").ToList();
            var authors = NormalizeAuthorsNames(link.InnerText);

            var articles = tags.Select(a =>
                new Article(date,
                a.Attributes["href"].Value,
                HttpUtility.HtmlDecode(a.InnerHtml),
                authors));
            return articles;
        }

        private string NormalizeAuthorsNames(string text)
        {
            text = text.Trim();
            var delimeter = " – ";
            text = text.Substring(text.LastIndexOf(delimeter) + delimeter.Length);
            var authors = text.Trim();

            return authors;
        }
    }
}