using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api/salary")]
public class SalaryController(ClaudeService claude, RagService rag) : ControllerBase
{
  [HttpPost("calculate")]
  public async Task<ActionResult<SalaryCalculateResponse>> Calculate([FromBody] SalaryCalculateRequest request, CancellationToken ct)
  {
    var ragResults = await rag.SearchAsync($"salary rates {request.Role} {request.Location}", ct: ct);
    var context = rag.BuildContext(ragResults);
    var profile = ProfileHelper.ToContext(request.Profile);

    var system = """
      You are a compensation analyst for US IT roles. Provide market rate ranges based on current data.
      Respond in JSON with keys: hourlyMin, hourlyMax, salaryMin, salaryMax, context, negotiationTips (array of strings).
      All monetary values as numbers (no $ signs). salaryMin/Max are annual full-time. hourlyMin/Max are contract C2C rates.
      """;
    var user = $"{profile}\n\nMarket context:\n{context}\n\nRole: {request.Role}\nLocation: {request.Location}\nExperience: {request.ExperienceLevel}\nType: {request.EmploymentType}";
    var result = await claude.CompleteJsonAsync<SalaryCalculateResponse>(system, user, ct);
    return Ok(result);
  }
}
