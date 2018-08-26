using System;
using System.Collections.Generic;
using System.Linq;
using Crawler.Entities;
using Crawler.PageProcessors;
using Crawler.Test.PageProcessors.HtmlPages;
using FluentAssertions;
using Xunit;

namespace Crawler.Test.PageProcessors
{
    [Trait(TraitConstants.Category, TraitConstants.Unit)]
    public class MorningBrewProcessorTest
    {
        readonly IEnumerable<Article> articles;
        public MorningBrewProcessorTest()
        {
            string page = ReadTextFromHtml.FromHtmlFile("morningbrew12072018.html");
            articles = GetArticles(page);
        }

        public IEnumerable<Article> GetArticles(string page)
        {
            IMorningBrewProcessor processor = new MorningBrewAgility();
            return processor.Map(page);
        }

        [Fact]
        public void CheckQuantityOfArticles()
            => articles.Should().HaveCount(22);

        [Fact]
        public void CheckContainPostDates()
        {
            articles.Should().Contain(p => p.Date.Date == new DateTime(2018, 7, 6));
            articles.Should().Contain(p => p.Date.Date == new DateTime(2018, 7, 5));
        }

        [Theory]
        [InlineData("New Azure #CosmosDB Explorer now in public preview")]
        [InlineData("What’s New in ASP.NET Core 2.1")]
        public void CheckContainTitles(string title)
            => articles.Should().Contain(p => p.Title == title);

        [Theory]
        [InlineData("Mark Thomas")]
        [InlineData("Scott Hanselman")]
        public void CheckContainAuthors(string author)
            => articles.SelectMany(s => s.Authors).Should().Contain(p => p.Name == author);

        [Fact]
        public void CheckAuthorsHasSameArticleId()
        {            
            foreach (var article in articles)
                article.Authors.Should().OnlyContain(o => o.ArticleId == article.Id);            
        }

        [Theory]
        [InlineData("http://mattwarren.org/2018/07/05/.NET-JIT-and-CLR-Joined-at-the-Hip/")]
        [InlineData("https://ayende.com/blog/183649-A/slides-from-todays-talk")]
        public void CheckContainLinks(string link)
            => articles.Should().Contain(p => p.Link == link);
    }
}