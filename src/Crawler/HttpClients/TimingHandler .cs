using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler.HttpClients
{
    public class TimingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();

            System.Console.WriteLine("Starting request");

            var response = await base.SendAsync(request, cancellationToken);

            System.Console.WriteLine($"Finished request in {sw.ElapsedMilliseconds}ms");

            return response;
        }
    }
}