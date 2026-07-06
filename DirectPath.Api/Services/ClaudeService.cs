using System.Text;
using System.Text.Json;
using DirectPath.Api.Config;

namespace DirectPath.Api.Services;

public class ClaudeService(HttpClient http, AppSettings settings)
{
  private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

  public async Task<string> CompleteAsync(
    string systemPrompt,
    string userMessage,
    List<(string Role, string Content)>? history = null,
    CancellationToken ct = default)
  {
    var messages = new List<object>();
    if (history != null)
    {
      foreach (var (role, content) in history)
        messages.Add(new { role, content });
    }
    messages.Add(new { role = "user", content = userMessage });

    var payload = new
    {
      model = AppSettings.ClaudeModel,
      max_tokens = 4096,
      system = systemPrompt,
      messages
    };

    using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
    req.Headers.Add("x-api-key", settings.AnthropicApiKey);
    req.Headers.Add("anthropic-version", "2023-06-01");
    req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

    var res = await http.SendAsync(req, ct);
    var body = await res.Content.ReadAsStringAsync(ct);
    if (!res.IsSuccessStatusCode)
      throw new InvalidOperationException($"Claude API error ({res.StatusCode}): {body}");

    using var doc = JsonDocument.Parse(body);
    var sb = new StringBuilder();
    foreach (var block in doc.RootElement.GetProperty("content").EnumerateArray())
    {
      if (block.GetProperty("type").GetString() == "text")
        sb.Append(block.GetProperty("text").GetString());
    }
    return sb.ToString();
  }

  public async Task<string> CompleteWithWebSearchAsync(string systemPrompt, string userMessage, CancellationToken ct = default)
  {
    var payload = new
    {
      model = AppSettings.ClaudeModel,
      max_tokens = 4096,
      system = systemPrompt,
      tools = new[] { new { type = "web_search_20250305", name = "web_search", max_uses = 5 } },
      messages = new[] { new { role = "user", content = userMessage } }
    };

    using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
    req.Headers.Add("x-api-key", settings.AnthropicApiKey);
    req.Headers.Add("anthropic-version", "2023-06-01");
    req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

    var res = await http.SendAsync(req, ct);
    var body = await res.Content.ReadAsStringAsync(ct);
    if (!res.IsSuccessStatusCode)
    {
      // Fallback without web search tool
      return await CompleteAsync(systemPrompt, userMessage, null, ct);
    }

    using var doc = JsonDocument.Parse(body);
    var sb = new StringBuilder();
    foreach (var block in doc.RootElement.GetProperty("content").EnumerateArray())
    {
      if (block.GetProperty("type").GetString() == "text")
        sb.Append(block.GetProperty("text").GetString());
    }
    return sb.ToString();
  }

  public async Task<T> CompleteJsonAsync<T>(string systemPrompt, string userMessage, CancellationToken ct = default)
  {
    var jsonPrompt = systemPrompt + "\n\nRespond ONLY with valid JSON. No markdown fences.";
    var text = await CompleteAsync(jsonPrompt, userMessage, null, ct);
    text = text.Trim();
    if (text.StartsWith("```"))
    {
      var lines = text.Split('\n').Skip(1).ToList();
      if (lines.Count > 0 && lines[^1].Trim() == "```") lines.RemoveAt(lines.Count - 1);
      text = string.Join('\n', lines);
    }
    return JsonSerializer.Deserialize<T>(text, JsonOpts)
      ?? throw new InvalidOperationException("Failed to parse Claude JSON response");
  }
}
