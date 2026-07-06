using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController(ClaudeService claude) : ControllerBase
{
  [HttpPost("build")]
  public async Task<ActionResult<ProfileBuildResponse>> Build([FromBody] ProfileBuildRequest request, CancellationToken ct)
  {
    var profile = ProfileHelper.ToContext(request.Profile);
    var system = """
      You are an expert technical recruiter. Build a structured candidate profile and bench pitch.
      Respond in JSON with keys: summary, skills, experience, education, certifications, benchPitch.
      The benchPitch should be how a recruiter would present this candidate to a client in 3-4 compelling sentences.
      """;
    var user = $"{profile}\n\nCandidate background:\n{request.Background}";
    var result = await claude.CompleteJsonAsync<ProfileBuildResponse>(system, user, ct);
    return Ok(result);
  }
}
