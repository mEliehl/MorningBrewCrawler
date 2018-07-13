using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crawler.Commands;
using Crawler.Entities;
using Crawler.HttpClients;
using Crawler.Repositories;
using Crawler.Test.PageProcessors.HtmlPages;
using Moq;
using Xunit;

namespace Crawler.Test.Commands
{
    public class CrawlerCommandHandlerTest
    {
        readonly ICommandHandler<CrawlerCommand> handler;
        readonly Mock<IArticles> mockArticles;

        public CrawlerCommandHandlerTest()
        {
            mockArticles = new Mock<IArticles>();
            mockArticles.Setup(s => s.Add(It.IsAny<IEnumerable<Article>>())).Returns(Task.CompletedTask);
            mockArticles.Setup(s => s.AnyWithDate(It.IsAny<DateTime>())).ReturnsAsync(false);

            var mochHttpHandler = new Mock<IMorningBrewClient>();
            mochHttpHandler.Setup(s => s.GetPageAsync(1)).ReturnsAsync(ReadTextFromHtml.FromHtmlFile("morningbrew06072018.html"));

            handler = new CrawlerCommandHandler(mochHttpHandler.Object,mockArticles.Object);
        }

        [Fact]
        public async Task WithOnlyNewArticles_CallAnyWithDateThreeTimes()
        {
            await handler.HandleAsync(new CrawlerCommand(1));

            mockArticles.Verify(s => s.Add(It.IsAny<IEnumerable<Article>>()), Times.Once());
            mockArticles.Verify(s => s.AnyWithDate(It.IsAny<DateTime>()),Times.Exactly(3));
        }

        [Fact]
        public async Task WithOnlyOldArticles_CallAnyWithDateOnce()
        {
            mockArticles.Setup(s => s.AnyWithDate(It.IsAny<DateTime>())).ReturnsAsync(true);
            
            await handler.HandleAsync(new CrawlerCommand(1));

            mockArticles.Verify(s => s.Add(It.IsAny<IEnumerable<Article>>()), Times.Once());
            mockArticles.Verify(s => s.AnyWithDate(It.IsAny<DateTime>()),Times.Once());
        }

        [Fact]
        public async Task WithWithNewAndOldArticles_CallAnyWithDateTwice()
        {
            mockArticles.SetupSequence(s => s.AnyWithDate(It.IsAny<DateTime>()))
                .ReturnsAsync(false)
                .ReturnsAsync(true);
            
            await handler.HandleAsync(new CrawlerCommand(1));

            mockArticles.Verify(s => s.Add(It.IsAny<IEnumerable<Article>>()), Times.Once());
            mockArticles.Verify(s => s.AnyWithDate(It.IsAny<DateTime>()),Times.Exactly(2));
        }
    }
}