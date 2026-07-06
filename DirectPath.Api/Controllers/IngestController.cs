using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api")]
public class IngestController(RagService rag, UrlScraperService scraper) : ControllerBase
{
  [HttpPost("ingest")]
  public async Task<ActionResult<IngestResponse>> Ingest([FromBody] IngestRequest request, CancellationToken ct)
  {
    var result = await rag.IngestAsync(request.Title, request.Text, request.Source ?? "manual", null, ct);
    return Ok(result);
  }

  [HttpPost("ingest-url")]
  public async Task<ActionResult<IngestResponse>> IngestUrl([FromBody] IngestUrlRequest request, CancellationToken ct)
  {
    var (title, text) = await scraper.ScrapeAsync(request.Url, ct);
    var result = await rag.IngestAsync(request.Title ?? title, text, "url", request.Url, ct);
    return Ok(result);
  }
}
