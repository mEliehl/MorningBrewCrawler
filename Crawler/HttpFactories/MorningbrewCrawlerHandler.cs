using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.HttpFactories
{
    public class MorningbrewHttpHandler : IHttpHandler
    {
        public async Task<string> HandleAsync(int page)
        {
            try
            {
                var url = $"http://blog.cwa.me.uk/page/{page}/";

                using (var client = new HttpClient())
                {
                    return await client.GetStringAsync(url);
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}