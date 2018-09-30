using Crawler.Entities;
using Crawler.PageProcessors;
using System.Collections.Generic;
using System.Web;

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