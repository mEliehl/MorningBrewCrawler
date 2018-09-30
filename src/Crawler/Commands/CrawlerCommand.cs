using Crawler.Entities;
using Crawler.HttpClients;
using Crawler.Mappers;
using Crawler.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Commands
{
    public class CrawlerCommand : ICommand
    {
        public CrawlerCommand(uint pageLimit) => PageLimit = pageLimit;
        public uint PageLimit { get; }

        public static implicit operator CrawlerCommand(uint pageLimit) => new CrawlerCommand(pageLimit);
    }

    class NewArticles
    {
        public List<Article> Articles { get; set; } = new List<Article>();
        public bool HasMore { get; set; }
    }

    public class CrawlerCommandHandler : ICommandHandler<CrawlerCommand>
    {
        readonly IMorningBrewClient morningBrewClient;
        readonly IArticles articles;

        public CrawlerCommandHandler(IMorningBrewClient morningBrewClient,
            IArticles articles)
        {
            this.morningBrewClient = morningBrewClient;
            this.articles = articles;
        }

        public async Task HandleAsync(CrawlerCommand command)
        {
            var entities = new List<Article>();
            for (var i = 1; i <= command.PageLimit; i++)
            {
                var page = await morningBrewClient.GetPageAsync(i);
                var extractedArticles = MorningBrewMapperPage.Map(page);
                var newArticles = await GetNewArticlesAsync(extractedArticles);
                entities.AddRange(newArticles.Articles);
                if (!newArticles.HasMore)
                    break;
            }
            await articles.Add(entities);
        }

        private async Task<NewArticles> GetNewArticlesAsync(IEnumerable<Article> entities)
        {
            var @return = new NewArticles() { HasMore = true };
            var groupedArticles = entities.OrderByDescending(o => o.Date).GroupBy(g => g.Date);
            foreach (var group in groupedArticles)
            {
                var postDate = group.FirstOrDefault().Date;
                if (await articles.AnyWithDate(postDate))
                {
                    @return.HasMore = false;
                    break;
                }
                @return.Articles.AddRange(group);
            }

            return @return;
        }
    }
}