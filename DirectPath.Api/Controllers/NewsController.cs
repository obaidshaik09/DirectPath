using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api/news")]
public class NewsController(NewsCacheService news) : ControllerBase
{
  [HttpPost("feed")]
  public async Task<ActionResult<NewsFeedResponse>> Feed(CancellationToken ct)
  {
    return Ok(await news.GetFeedAsync(ct));
  }

  [HttpPost("search")]
  public async Task<ActionResult<List<NewsItem>>> Search([FromBody] NewsSearchRequest request, CancellationToken ct)
  {
    return Ok(await news.SearchAsync(request.Keyword, ct));
  }
}
