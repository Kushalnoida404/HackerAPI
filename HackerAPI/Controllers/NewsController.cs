using Hacker.Core.News;
using Microsoft.AspNetCore.Mvc;

namespace HackerAPI.Controllers;


[Route("news")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;
    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    /// <summary>
    /// Get Latest News
    /// </summary>
    /// <param name="cancellationToken">Pass Cancellation Token</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetLatestNewsAsync(CancellationToken cancellationToken)
    {
        var result = await _newsService.GetNewsAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

}
