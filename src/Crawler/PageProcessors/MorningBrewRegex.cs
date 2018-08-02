using System.Collections.Generic;
using System.Web;
using Crawler.Entities;

namespace Crawler.PageProcessors
{
    public class MorningBrewRegex : IMorningBrewProcessor
    {
        public IEnumerable<Article> Map(string page)
        {
            page = HttpUtility.HtmlDecode(page);

            return default;
        }
    }
}