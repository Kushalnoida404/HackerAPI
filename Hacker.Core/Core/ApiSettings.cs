namespace Hacker.Core.Core;

public enum Authorization
{
    None = 0,
    LiveToken = 1,
    OAuth = 2
}

public class ApiSettings<T> where T : class
{
    public string BaseUrl { get; set; } = string.Empty;
    public Authorization Authorization { get; set; } = Authorization.None;
    public T Endpoints { get; set; }    = default!;
}
