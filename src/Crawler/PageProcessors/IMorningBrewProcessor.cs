using System.Collections.Generic;
using Crawler.Entities;

namespace Crawler.PageProcessors
{
    public interface IMorningBrewProcessor
    {
        IEnumerable<Article> Map(string page);
    }
}