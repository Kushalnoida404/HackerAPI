namespace Hacker.Core.News;

public class News
{
    public int NewsId { get; set; } = 0;
}

public class NewsItems
{
    public int Id { get; set; } = 0;
    public bool Deleted { get; set; } = false;
    public string Type { get; set; } = string.Empty;
    public string By { get; set; } = string.Empty ;
    public string Text { get; set; } = string.Empty;

    public string Dead { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

}
