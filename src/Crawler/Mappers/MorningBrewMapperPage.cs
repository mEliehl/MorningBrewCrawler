using System.Collections.Generic;
using System.Web;
using Crawler.Entities;
using Crawler.PageProcessors;

namespace Crawler.Mappers
{
    public class MorningBrewMapperPage
    {
        public static IEnumerable<Article> Map(string page)
        {
            IMorningBrewProcessor processor = new MorningBrewAgility();
            page = HttpUtility.HtmlDecode(page);
            return processor.Map(page);
        }
    }
}