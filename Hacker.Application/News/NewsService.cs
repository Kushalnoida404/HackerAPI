using Hacker.Core.News;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Hacker.Application.News;

public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;
    private readonly IDistributedCache _distributedCache;
    private const int MaxNewsItems = 200;
    public NewsService(INewsRepository newsRepository, IDistributedCache distributedCache)
    {
        _newsRepository = newsRepository;
        _distributedCache = distributedCache;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Core.News.NewsItems>> GetNewsAsync(CancellationToken cancellationToken = default)
    {
        List<Core.News.News> news = await _newsRepository.GetNewsAsync(cancellationToken);
        List<Core.News.NewsItems> newsItems = new List<Core.News.NewsItems>();
        int newsLength = news.Count > MaxNewsItems ? MaxNewsItems : news.Count;
        var tasks = new List<Task<NewsItems>>();

        for (int i = 0; i < newsLength; i++)
        {
            var Id = news[i].NewsId;

           byte[] encodedItem =  await _distributedCache.GetAsync(Id.ToString());
            if(encodedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(encodedItem);
                var item = JsonSerializer.Deserialize<NewsItems>(serializedItem);
                newsItems.Add(item);
                continue;
            }
            else
            {
                tasks.Add(Task.Run(() => _newsRepository.GetNewsItemsAsync(Id, cancellationToken)));
            }
            

        }
        NewsItems[] result = await Task.WhenAll(tasks);

        foreach (var item in result)
        {
            newsItems.Add(item);
            var serializedItem = JsonSerializer.Serialize(item);    
            await _distributedCache.SetAsync(item.Id.ToString(), Encoding.UTF8.GetBytes(serializedItem));
            
        }

        return newsItems;
    }
}
