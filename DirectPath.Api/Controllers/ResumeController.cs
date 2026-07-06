using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api/resume")]
public class ResumeController(ClaudeService claude, RagService rag) : ControllerBase
{
  [HttpPost("optimize")]
  public async Task<ActionResult<ResumeOptimizeResponse>> Optimize([FromBody] ResumeOptimizeRequest request, CancellationToken ct)
  {
    var ragResults = await rag.SearchAsync("ATS resume optimization keywords " + request.JobDescription[..Math.Min(200, request.JobDescription.Length)], ct: ct);
    var context = rag.BuildContext(ragResults);
    var profile = ProfileHelper.ToContext(request.Profile);

    var system = """
      You are an ATS resume optimization expert. Rewrite the resume to maximize ATS match for the job description.
      Respond in JSON with keys:
      - optimizedResume (full rewritten resume text)
      - keywordAnalysis (array of {keyword, action: "added"|"removed"|"changed", reason})
      - summary (brief explanation of key changes)
      Maintain truthfulness — only reframe existing experience, never fabricate.
      """;
    var user = $"{profile}\n\nKnowledge base tips:\n{context}\n\nRESUME:\n{request.ResumeText}\n\nJOB DESCRIPTION:\n{request.JobDescription}";
    var result = await claude.CompleteJsonAsync<ResumeOptimizeResponse>(system, user, ct);
    return Ok(result);
  }
}
