using Hacker.Application.News;
using Hacker.Core.News;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;
using System.Text.Json;


namespace Hacker.Application.Tests.News
{
    public class NewsServiceTests
    {
        private readonly Mock<INewsRepository> _newsRepositoryMock;
        private readonly Mock<IDistributedCache> _distributedCacheMock;
        private readonly NewsService _newsService;

        public NewsServiceTests()
        {
            _newsRepositoryMock = new Mock<INewsRepository>();
            _distributedCacheMock = new Mock<IDistributedCache>();
            _newsService = new NewsService(_newsRepositoryMock.Object, _distributedCacheMock.Object);
        }

        [Fact]
        public async Task GetNewsAsync_Should_Return_NewsItems()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var news = new List<Hacker.Core.News.News>
            {
                new Hacker.Core.News.News { NewsId = 1 },
                new Hacker.Core.News.News { NewsId = 2 },
                new Hacker.Core.News.News { NewsId = 3 }
            };
            var newsItems = new List<NewsItems>
            {
                new NewsItems { Id = 1, Title = "News 1" },
                new NewsItems { Id = 2, Title = "News 2" },
                new NewsItems { Id = 3, Title = "News 3" }
            };
            var serializedItem1 = JsonSerializer.Serialize(newsItems[0]);
            var serializedItem2 = JsonSerializer.Serialize(newsItems[1]);
            var serializedItem3 = JsonSerializer.Serialize(newsItems[2]);
            var encodedItem1 = Encoding.UTF8.GetBytes(serializedItem1);
            var encodedItem2 = Encoding.UTF8.GetBytes(serializedItem2);
            var encodedItem3 = Encoding.UTF8.GetBytes(serializedItem3);

            _newsRepositoryMock.Setup(repo => repo.GetNewsAsync(cancellationToken)).ReturnsAsync(news);
            
            _distributedCacheMock.Setup(cache => cache.GetAsync("1", cancellationToken)).ReturnsAsync(encodedItem1);
            _distributedCacheMock.Setup(cache => cache.GetAsync("2", cancellationToken)).ReturnsAsync(encodedItem2);
            _distributedCacheMock.Setup(cache => cache.GetAsync("3", cancellationToken)).ReturnsAsync(encodedItem3);
            _newsRepositoryMock.Setup(repo => repo.GetNewsItemsAsync(1, cancellationToken)).ReturnsAsync(newsItems[0]);
            _newsRepositoryMock.Setup(repo => repo.GetNewsItemsAsync(2, cancellationToken)).ReturnsAsync(newsItems[1]);
            _newsRepositoryMock.Setup(repo => repo.GetNewsItemsAsync(3, cancellationToken)).ReturnsAsync(newsItems[2]);

            // Act
            var result = await _newsService.GetNewsAsync(cancellationToken);

            // Assert
            Assert.Equal(newsItems.Count, result.ToList().Count);
        }
    }
}
