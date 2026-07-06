using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api/interview")]
public class InterviewController(ClaudeService claude, RagService rag) : ControllerBase
{
  [HttpPost("prep")]
  public async Task<ActionResult<InterviewPrepResponse>> Prep([FromBody] InterviewPrepRequest request, CancellationToken ct)
  {
    var ragResults = await rag.SearchAsync($"interview preparation {request.Role} STAR method", ct: ct);
    var context = rag.BuildContext(ragResults);
    var profile = ProfileHelper.ToContext(request.Profile);

    var system = """
      You are an interview coach. Generate tailored interview questions with STAR method coaching.
      Respond in JSON with key: questions (array of {question, type: "technical"|"behavioral", starCoaching}).
      Include 5-7 technical and 5-7 behavioral questions specific to the role and company.
      """;
    var user = $"{profile}\n\nInterview knowledge:\n{context}\n\nRole: {request.Role}\nCompany: {request.Company}";
    var result = await claude.CompleteJsonAsync<InterviewPrepResponse>(system, user, ct);
    return Ok(result);
  }
}
