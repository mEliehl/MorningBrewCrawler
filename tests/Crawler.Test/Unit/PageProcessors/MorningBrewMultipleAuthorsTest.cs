using Crawler.Entities;
using Crawler.PageProcessors;
using Crawler.Test.PageProcessors.HtmlPages;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Crawler.Test.PageProcessors
{
    [Trait(TraitConstants.Category, TraitConstants.Unit)]
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
        public void CheckAuthorsNames()
        {
            article.Authors.Select(s => s.Name)
                .Should()
                .BeEquivalentTo(new string[]{
                    "Anthony Cangialosi", "Robert Green", "Amanda Silver", "Kendra Havens"
                });
        }
    }
}