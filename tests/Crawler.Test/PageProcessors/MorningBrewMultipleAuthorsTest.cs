using System.Collections.Generic;
using System.Linq;
using Crawler.Entities;
using Crawler.PageProcessors;
using Crawler.Test.PageProcessors.HtmlPages;
using FluentAssertions;
using Xunit;

namespace Crawler.Test.PageProcessors
{
    public class MorningBrewMultipleAuthorsTest
    {
        readonly Article article;
        public MorningBrewMultipleAuthorsTest()
        {
            string page = ReadTextFromHtml.FromHtmlFile("morningbrew04072018.html");
            article = GetArticleWithMultipleAuthors(page);
        }

        private Article GetArticleWithMultipleAuthors(string page)
        {
            IMorningBrewProcessor processor = new MorningBrewAgility();
            var articles = processor.Map(page);

            return articles.FirstOrDefault(f => f.Title == "The Present and Not-too-distant Future of Visual Studio Part 1 – Visual Studio Toolbox");
        }

        [Fact]
        public void CheckAuthors()
        {
            article.Authors.Should().Equals("Anthony Cangialosi, Robert Green, Amanda Silver & Kendra Havens");
        }
    }
}