using System;
using System.Collections.Generic;
using System.IO;
using Crawler.Entities;
using Crawler.Mappers;
using Crawler.Test.Mappers.HtmlPages;
using FluentAssertions;
using Xunit;

namespace Crawler.Test.Mappers
{
    public class MorningBrewPageTest
    {
        readonly IEnumerable<Article> articles;

        public MorningBrewPageTest()
        {
            string page = ReadTextFromHtml.FromHtmlFile("morningbrew12072018.html");

            articles = MorningBrewMapperPage.Map(page);
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