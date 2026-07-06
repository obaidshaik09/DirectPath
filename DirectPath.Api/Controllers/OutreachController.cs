using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api/outreach")]
public class OutreachController(ClaudeService claude, RagService rag) : ControllerBase
{
  [HttpPost("generate")]
  public async Task<ActionResult<OutreachGenerateResponse>> Generate([FromBody] OutreachGenerateRequest request, CancellationToken ct)
  {
    var ragResults = await rag.SearchAsync("LinkedIn outreach messages hiring managers", ct: ct);
    var context = rag.BuildContext(ragResults);
    var profile = ProfileHelper.ToContext(request.Profile);

    var system = """
      You are a LinkedIn outreach expert helping candidates contact hiring managers directly.
      Respond in JSON with keys: connectionMessage (under 300 chars), followUpMessages (array of exactly 3 messages for day 3, day 7, day 14).
      Messages should be personalized, professional, and not salesy.
      """;
    var user = $"{profile}\n\nOutreach tips:\n{context}\n\nTarget company: {request.TargetCompany}\nRole: {request.Role}\nBackground: {request.CandidateBackground}";
    var result = await claude.CompleteJsonAsync<OutreachGenerateResponse>(system, user, ct);
    return Ok(result);
  }
}
