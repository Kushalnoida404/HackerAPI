﻿using Hacker.Core.Core;
using Hacker.Core.News;

namespace Hacker.Application.News;

public class NewsRepository : INewsRepository
{
    private readonly IApiClient<HackerApiEndpoints> _apiClient;

    public NewsRepository(IApiClient<HackerApiEndpoints> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<Core.News.News>> GetNewsAsync(CancellationToken cancellationToken = default)
    {
        var content = await _apiClient.GetAsString(_apiClient.Endpoints.Stories, cancellationToken).ConfigureAwait(false);
        if (content == null) {
            throw new AppException($"No News found");
        }
        content = content.ReplaceArrayBraket();

        List<Core.News.News> news = new List<Core.News.News>();
        foreach (var item in content.Split(',')) {
            news.Add(new Core.News.News { NewsId = Int32.Parse(item) });
        }
        return news;
    }

    public async Task<NewsItems> GetNewsItemsAsync(int newsId, CancellationToken cancellationToken = default)
    {
        var urlParameters = new NameValueList<object> { { "newsId", newsId } };
        var content = await _apiClient.Get<NewsItems>(_apiClient.Endpoints.Detail, urlParameters, cancellationToken).ConfigureAwait(false);
        return content!;
    }
}