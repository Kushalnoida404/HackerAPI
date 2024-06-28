using Hacker.Core.News;
using HackerAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HackerAPI.Tests.Controllers;

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
        var news = new List<NewsItems>();

        news.Add(new NewsItems { Id = 1, Title = "Test News 1" });
        news.Add(new NewsItems { Id = 2, Title = "Test News 2" });

        _newsServiceMock.Setup(x => x.GetNewsAsync(cancellationToken)).ReturnsAsync(news);

        // Act
        var result = await _newsController.GetLatestNewsAsync(cancellationToken);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}
