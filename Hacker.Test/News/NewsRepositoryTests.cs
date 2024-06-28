using Hacker.Application.News;
using Hacker.Core.Core;
using Hacker.Core.News;
using Moq;

namespace Hacker.Application.Tests.News;

public class NewsRepositoryTests
{
    private readonly Mock<IApiClient<HackerApiEndpoints>> _apiClientMock;
    private readonly NewsRepository _newsRepository;

    public NewsRepositoryTests()
    {
        _apiClientMock = new Mock<IApiClient<HackerApiEndpoints>>();
        _newsRepository = new NewsRepository(_apiClientMock.Object);
    }

    [Fact]
    public async Task GetNewsAsync_ShouldReturnListOfNews()
    {
        // Arrange
        var content = "[1,2,3,4,5]";
        _apiClientMock.Setup(x => x.GetAsString(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(content);
        _apiClientMock.Setup(x => x.Endpoints).Returns(new HackerApiEndpoints() { Detail= "/v0/item/{newsId}.json?print=pretty", Stories= "/v0/newstories.json" });
        // Act
        var result = await _newsRepository.GetNewsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Core.News.News>>(result);
        Assert.Equal(5, result.Count);
        for (int i = 0; i < result.Count; i++)
        {
            Assert.Equal(i + 1, result[i].NewsId);
        }
    }

    [Fact]
    public async Task GetNewsAsync_ShouldThrowAppException_WhenContentIsNull()
    {
        // Arrange
        string content = null;
        _apiClientMock.Setup(x => x.GetAsString(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(content);
        _apiClientMock.Setup(x => x.Endpoints).Returns(new HackerApiEndpoints() { Detail = "/v0/item/{newsId}.json?print=pretty", Stories = "/v0/newstories.json" });

        // Act & Assert
        await Assert.ThrowsAsync<AppException>(() => _newsRepository.GetNewsAsync());
    }

    [Fact]
    public async Task GetNewsItemsAsync_ShouldReturnNewsItems()
    {
        // Arrange
        int newsId = 1;
        var newsItems = new NewsItems { Id = newsId, Text="Test" };
        _apiClientMock.Setup(x => x.Get<NewsItems>(It.IsAny<string>(), It.IsAny<NameValueList<object>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newsItems);
        _apiClientMock.Setup(x => x.Endpoints).Returns(new HackerApiEndpoints() { Detail = "/v0/item/{newsId}.json?print=pretty", Stories = "/v0/newstories.json" });

        // Act
        var result = await _newsRepository.GetNewsItemsAsync(newsId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newsId, result.Id);
    }
}
