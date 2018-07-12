using System.Threading.Tasks;

namespace Crawler.HttpFactories
{
    public interface IHttpHandler
    {
        Task<string> HandleAsync(int page);
    }
}