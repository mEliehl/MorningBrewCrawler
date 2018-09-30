using Crawler.Entities;
using System.Collections.Generic;

namespace Crawler.PageProcessors
{
    public interface IMorningBrewProcessor
    {
        IEnumerable<Article> Map(string page);
    }
}