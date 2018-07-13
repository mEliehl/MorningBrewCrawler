using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.HttpClients
{
    public class MorningBrewClient : IMorningBrewClient
    {
        private readonly HttpClient client;

        public MorningBrewClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> GetPageAsync(int page)
        {
             return await client.GetStringAsync($"/page/{page}/");
        }
    }
}