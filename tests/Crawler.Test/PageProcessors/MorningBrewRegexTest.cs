using System.Collections.Generic;
using Crawler.Entities;
using Crawler.PageProcessors;

namespace Crawler.Test.PageProcessors
{
    public class MorningBrewRegexTest : MorningBrewProcessorTest
    {
        protected override IEnumerable<Article> GetArticles(string page)
        {
            IMorningBrewProcessor processor = new MorningBrewRegex();
            return processor.Map(page);
        }
    }
}