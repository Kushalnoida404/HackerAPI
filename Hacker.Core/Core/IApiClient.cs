namespace Hacker.Core.Core;

public interface IApiClient<TEndpoints> where TEndpoints : class
{
    TEndpoints Endpoints { get; }
    Task<TResult?> Get<TResult>(string url, NameValueList<object>? urlParameters, CancellationToken cancellationToken = default);
    Task<string> GetAsString(string url, CancellationToken cancellationToken = default);
    Task<TResult?> Post<TResult>(string url, object content, CancellationToken cancellationToken = default);
}
