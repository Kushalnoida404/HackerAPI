namespace Hacker.Core.News;

public interface INewsService
{
    Task<IEnumerable<NewsItems>> GetNewsAsync(CancellationToken cancellationToken = default);
}


public interface INewsRepository
{
    Task<List<News>> GetNewsAsync(CancellationToken cancellationToken = default);

    Task<NewsItems> GetNewsItemsAsync(int newsId,CancellationToken cancellationToken = default);
}