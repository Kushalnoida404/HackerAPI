using Hacker.Core.Core;

namespace HackerAPI.Infrastructure;

public class ApiClient<TEndpoints> : IApiClient<TEndpoints> where TEndpoints : class
{

    private const string AuthorizationHeader = "Authorization";

    private readonly ApiSettings<TEndpoints> _settings;
    private readonly Uri _baseUri;
    private readonly IHttpClientFactory _httpClientFactory;

    private string _authorization = string.Empty;
    private DateTime _authorizationExpiry = DateTime.MinValue;

    public ApiClient(ApiSettings<TEndpoints> settings, IHttpClientFactory httpClientFactory)
    {
        _settings = settings;
        _baseUri = new Uri(_settings.BaseUrl);
        _httpClientFactory = httpClientFactory;
    }
    public TEndpoints Endpoints => _settings.Endpoints;

    public async Task<TResult?> Get<TResult>(string url, NameValueList<object>? urlParameters, CancellationToken cancellationToken = default)
    {
        var requestUrl = url.ReplaceTokens(urlParameters);
        var client = BuildClientAsync(cancellationToken);
        using var response = await client.GetAsync(requestUrl, cancellationToken).ConfigureAwait(false);

        try
        {
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TResult?>(cancellationToken: cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            throw await ApiClient<TEndpoints>.CreateExceptionAsync($"Unable to call {requestUrl}", client, response, ex).ConfigureAwait(false);
        }
    }

    public async Task<string> GetAsString(string url, CancellationToken cancellationToken = default)
    {
        var client = BuildClientAsync(cancellationToken);
        using var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);

        try
        {
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(false);           
            return result;
        }
        catch (Exception ex)
        {
            throw await ApiClient<TEndpoints>.CreateExceptionAsync($"Unable to call {url}", client, response, ex).ConfigureAwait(false);
        }
    }

    public Task<TResult?> Post<TResult>(string url, object content, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private  HttpClient BuildClientAsync(CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_settings.BaseUrl);
        return client;
    }

    private static async Task<ApiClientException> CreateExceptionAsync(string message, HttpClient? client, HttpResponseMessage? response, Exception exception)
    {
        var ex = new ApiClientException(message, exception)
              .AddData("SettingsType", typeof(TEndpoints).Name);

        if (client != null)
        {
            var clientAuthorization = client.DefaultRequestHeaders.FirstOrDefault(h => h.Key.IsEqual(AuthorizationHeader)).Value;
            ex.AddData("BaseAddress", client.BaseAddress)
              .AddData("ClientAuthorization", clientAuthorization.Join());
        }

        if (response != null)
        {
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            ex.AddData("StatusCode", response.StatusCode)
              .AddData("ReasonPhrase", response.ReasonPhrase ?? string.Empty)
              .AddData("ResponseBody", responseBody);

            if (response.RequestMessage != null)
            {
                var requestBody = response.RequestMessage.Content != null
                    ? await response.RequestMessage.Content.ReadAsStringAsync().ConfigureAwait(false)
                    : string.Empty;

                ex.AddData("Url", response.RequestMessage.RequestUri?.ToString())
                  .AddData("Method", response.RequestMessage.Method)
                  .AddData("RequestAuthorization", response.RequestMessage.Headers?.Authorization?.ToString())
                  .AddData("RequestBody", requestBody);
            }
        }

        return ex;
    }
}
