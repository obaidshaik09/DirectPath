using DirectPath.Api.Models;
using DirectPath.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectPath.Api.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController(RagService rag, ClaudeService claude) : ControllerBase
{
  [HttpPost]
  public async Task<ActionResult<ChatResponse>> Post([FromBody] ChatRequest request, CancellationToken ct)
  {
    var results = await rag.SearchAsync(request.Message, ct: ct);
    var ragContext = rag.BuildContext(results);
    var profile = ProfileHelper.ToContext(request.Profile);

    var system = """
      You are DirectPath, an AI-powered personal recruiter helping IT candidates find jobs independently.
      You have insider recruitment knowledge. Be practical, specific, and actionable.
      If context from the knowledge base is provided, prioritize it in your answer.
      """;

    var userMsg = request.Message;
    if (!string.IsNullOrEmpty(profile)) userMsg = profile + "\n\n" + userMsg;
    if (!string.IsNullOrEmpty(ragContext))
      userMsg = $"Knowledge base context (use this first):\n{ragContext}\n\nQuestion: {request.Message}";

    var history = request.History?.Select(h => (h.Role, h.Content)).ToList();
    var reply = await claude.CompleteAsync(system, userMsg, history, ct);

    return Ok(new ChatResponse(reply, results.Count > 0, results.Select(r => r.Chunk.Content[..Math.Min(80, r.Chunk.Content.Length)] + "...").ToList()));
  }
}
