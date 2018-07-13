using System;
using System.Collections.Generic;
using Crawler.Entities;
using Crawler.PageProcessors;
using Crawler.Test.PageProcessors.HtmlPages;
using FluentAssertions;
using Xunit;

namespace Crawler.Test.PageProcessors
{
    public class MorningBrewAgilityProcessorTest
    {
        readonly IEnumerable<Article> articles;
        public MorningBrewAgilityProcessorTest()
        {
            string page = ReadTextFromHtml.FromHtmlFile("morningbrew12072018.html");
            IMorningBrewProcessor processor = new MorningBrewAgility();
            articles = processor.Map(page);
        }

        [Fact]
        public void WithSpecifPage_CheckQuantityOfArticles()
        {
            articles.Should().HaveCount(22);
        }

        [Fact]
        public void WithSpecifPage_CheckContainPostDates()
        {
            articles.Should().Contain(p => p.Date.Date == new DateTime(2018,7,6));
            articles.Should().Contain(p => p.Date.Date == new DateTime(2018,7,5));
        }

        [Theory]
        [InlineData("New Azure #CosmosDB Explorer now in public preview")]
        [InlineData("What’s New in ASP.NET Core 2.1")]
         public void WithSpecifPage_CheckContainTitles(string title)
        {
            articles.Should().Contain(p => p.Title == title);
        }

        [Theory]
        [InlineData("Mark Thomas")]
        [InlineData("Scott Hanselman")]
         public void WithSpecifPage_CheckContainAuthors(string author)
        {
            articles.Should().Contain(p => p.Authors == author);
        }

        [Theory]
        [InlineData("http://mattwarren.org/2018/07/05/.NET-JIT-and-CLR-Joined-at-the-Hip/")]
        [InlineData("https://ayende.com/blog/183649-A/slides-from-todays-talk")]
         public void WithSpecifPage_CheckContainLinks(string link)
        {
            articles.Should().Contain(p => p.Link == link);
        }
    }
}