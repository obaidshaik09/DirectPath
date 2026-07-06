using DirectPath.Api.Data;
using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController(DirectPathDbContext db, AutoPopulateService autoPopulate) : ControllerBase
{
  [HttpGet("stats")]
  public async Task<ActionResult<AdminStatsResponse>> Stats(CancellationToken ct)
  {
    var docs = await db.Documents.CountAsync(ct);
    var chunks = await db.Chunks.CountAsync(ct);
    return Ok(new AdminStatsResponse(docs, chunks));
  }

  [HttpPost("auto-populate")]
  public async Task<ActionResult<AdminStatsResponse>> AutoPopulate(CancellationToken ct)
  {
    var total = await autoPopulate.PopulateAsync(ct);
    var docs = await db.Documents.CountAsync(ct);
    return Ok(new AdminStatsResponse(docs, total));
  }
}
