using Crawler.Entities;
using Crawler.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Crawler.PageProcessors
{
    public class MorningBrewRegex : IMorningBrewProcessor
    {
        public IEnumerable<Article> Map(string page)
        {
            page = NormalizeString(page);
            var @return = new List<Article>();

            var posts = ExtractPosts(page);

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

        private string NormalizeString(string page)
        {
            page = HttpUtility.HtmlDecode(page);
            page = Regex.Replace(page, @"\r\n?|\n", string.Empty);
            return page;
        }

        private IEnumerable<string> ExtractPosts(string page)
        {
            var matches = Regex.Matches(page, "<div class=\"post\">.+?<!--");

            return matches
                .Cast<Match>()
                .Select(m => m.Value.Replace("<!--", string.Empty))
                .ToList();
        }

        private DateTime ExtractPostDate(string post)
        {
            var p = Regex.Match(post, "<p class=\"day-date\">.*?</p>");
            var em = Regex.Matches(p.Value, "<em>.*?</em>");
            var postDate = em[1].Value;
            postDate = postDate.Replace("<em>", string.Empty);
            postDate = postDate.Replace("</em>", string.Empty);

            return MountDate(postDate);
        }

        private DateTime MountDate(string postDate)
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

        private IEnumerable<string> ExtractLinks(string post)
        {
            return Regex.Matches(post, "<li>.*?</li>")
                .Cast<Match>()
                .Select(s => s.Value
                    .Replace("<li>", string.Empty)
                    .Replace("</li>", string.Empty));
        }

        private IEnumerable<Article> ExtractArticles(string link, DateTime date)
        {
            var authors = ExtractAuthorsNames(link);
            var tags = Regex.Matches(link, "<a.+?</a>").Cast<Match>().Select(s => s.Value);

            var articles = tags.Select(a =>
                new Article(date,
                ExtractHREF(a),
                ExtractTitle(a),
                authors));
            return articles;
        }

        private string ExtractAuthorsNames(string link)
        {
            link = link.Trim();
            link = Regex.Match(link, "</a>.*").Value.Replace("</a>", string.Empty);
            var delimeter = " – ";
            link = link.Substring(link.LastIndexOf(delimeter) + delimeter.Length);
            var authors = link.Trim();

            return authors;
        }

        private string ExtractHREF(string tag)
        {
            return Regex.Match(tag, "href=\".+?\"").Value.Replace("href=\"", string.Empty).Replace("\"", string.Empty);
        }

        private string ExtractTitle(string tag)
        {
            return Regex.Match(tag, ">.+?<").Value.Replace("<", string.Empty).Replace(">", string.Empty);
        }
    }
}