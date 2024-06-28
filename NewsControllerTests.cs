using System.Threading;
using System.Threading.Tasks;
using Hacker.Core.News;
using HackerAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HackerAPI.Tests.Controllers
{
    public class NewsControllerTests
    {
        private readonly Mock<INewsService> _newsServiceMock;
        private readonly NewsController _newsController;

        public NewsControllerTests()
        {
            _newsServiceMock = new Mock<INewsService>();
            _newsController = new NewsController(_newsServiceMock.Object);
        }

        [Fact]
        public async Task GetLatestNewsAsync_ReturnsOkResult()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var news = new[] { "News 1", "News 2", "News 3" };
            _newsServiceMock.Setup(x => x.GetNewsAsync(cancellationToken)).ReturnsAsync(news);

            // Act
            var result = await _newsController.GetLatestNewsAsync(cancellationToken);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
