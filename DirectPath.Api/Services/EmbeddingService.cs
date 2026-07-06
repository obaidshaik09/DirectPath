using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DirectPath.Api.Config;
using Pgvector;

namespace DirectPath.Api.Services;

public class EmbeddingService(HttpClient http, AppSettings settings)
{
  private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

  public async Task<Vector> EmbedAsync(string text, CancellationToken ct = default)
  {
    var payload = new
    {
      model = AppSettings.EmbeddingModel,
      input = text
    };

    using var req = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/embeddings");
    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.OpenRouterApiKey);
    req.Headers.Add("HTTP-Referer", "https://directpath.app");
    req.Headers.Add("X-Title", "DirectPath");
    req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

    var res = await http.SendAsync(req, ct);
    res.EnsureSuccessStatusCode();
    var json = await res.Content.ReadAsStringAsync(ct);
    using var doc = JsonDocument.Parse(json);
    var arr = doc.RootElement.GetProperty("data")[0].GetProperty("embedding");
    var floats = arr.EnumerateArray().Select(e => e.GetSingle()).ToArray();
    return new Vector(floats);
  }

  public async Task<List<Vector>> EmbedBatchAsync(IEnumerable<string> texts, CancellationToken ct = default)
  {
    var list = texts.ToList();
    var results = new List<Vector>();
    foreach (var text in list)
      results.Add(await EmbedAsync(text, ct));
    return results;
  }
}
