using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace DirectPath.Api.Services;

public class UrlScraperService(HttpClient http)
{
  public async Task<(string Title, string Text)> ScrapeAsync(string url, CancellationToken ct = default)
  {
    using var req = new HttpRequestMessage(HttpMethod.Get, url);
    req.Headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; DirectPath/1.0)");
    var res = await http.SendAsync(req, ct);
    res.EnsureSuccessStatusCode();
    var html = await res.Content.ReadAsStringAsync(ct);

    var doc = new HtmlDocument();
    doc.LoadHtml(html);

    var title = doc.DocumentNode.SelectSingleNode("//title")?.InnerText?.Trim() ?? url;
    title = Regex.Replace(title, @"\s+", " ");

    foreach (var node in doc.DocumentNode.SelectNodes("//script|//style|//nav|//footer|//header") ?? Enumerable.Empty<HtmlNode>())
      node.Remove();

    var body = doc.DocumentNode.SelectSingleNode("//body") ?? doc.DocumentNode;
    var text = Regex.Replace(body.InnerText, @"\s+", " ").Trim();
    return (title, text.Length > 50000 ? text[..50000] : text);
  }
}
