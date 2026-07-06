using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController(WebSearchService webSearch) : ControllerBase
{
  [HttpPost("search")]
  public async Task<ActionResult<JobSearchResponse>> Search([FromBody] JobSearchRequest request, CancellationToken ct)
  {
    var jobs = await webSearch.SearchJobsAsync(request, ct);
    return Ok(new JobSearchResponse(jobs));
  }
}
