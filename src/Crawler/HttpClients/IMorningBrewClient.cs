using System.Threading.Tasks;

namespace Crawler.HttpClients
{
    public interface IMorningBrewClient
    {
         Task<string> GetPageAsync(int page);
    }
}