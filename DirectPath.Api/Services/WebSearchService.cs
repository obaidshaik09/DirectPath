using System.Text.Json;
using System.Text.RegularExpressions;
using DirectPath.Api.Models;

namespace DirectPath.Api.Services;

public class WebSearchService(ClaudeService claude)
{
  public async Task<List<JobListing>> SearchJobsAsync(JobSearchRequest request, CancellationToken ct = default)
  {
    var skills = request.Skills ?? request.Profile?.Role ?? "software engineer";
    var location = request.Location ?? request.Profile?.Location ?? "United States";
    var empType = request.EmploymentType ?? request.Profile?.EmploymentType ?? "full time and contract";
    var visa = request.VisaRequirement ?? request.Profile?.WorkAuthorization ?? "any";

    var prompt = $$"""
      Search the web for real, currently open IT job postings in the United States.
      Skills/role: {{skills}}
      Location: {{location}}
      Employment type: {{empType}} (include both full-time W2 and contract C2C/1099 if applicable)
      Visa/work authorization filter: {{visa}}

      Return a JSON array of 8-12 real job listings with this exact structure:
      [{"company":"...","title":"...","location":"...","employmentType":"Full-time|Contract|Contract-to-hire","applyUrl":"https://...","description":"brief 1-2 sentence description"}]

      Only include jobs with real apply URLs. Prefer LinkedIn, Indeed, Dice, or company career pages.
      """;

  var system = "You are a job search assistant. Use web search to find real current job postings. Return only valid JSON array.";
    var raw = await claude.CompleteWithWebSearchAsync(system, prompt, ct);

    try
    {
      return ParseJsonArray<JobListingDto>(raw)
        .Select(j => new JobListing(j.Company, j.Title, j.Location, j.EmploymentType, j.ApplyUrl, j.Description))
        .Where(j => !string.IsNullOrWhiteSpace(j.ApplyUrl))
        .ToList();
    }
    catch
    {
      return [];
    }
  }

  public async Task<List<NewsItem>> FetchNewsByCategoryAsync(string category, CancellationToken ct = default)
  {
    var query = category switch
    {
      "hiring_trends" => "US tech hiring trends 2025 2026 software engineering recruitment",
      "layoffs" => "US tech layoffs 2025 2026 IT companies job cuts",
      "in_demand_skills" => "most in demand IT skills 2025 2026 software development",
      "salary_trends" => "US tech salary trends 2025 2026 software engineer compensation",
      "visa_updates" => "US H1B visa work authorization IT workers 2025 2026 updates",
      "remote_work" => "remote work trends IT jobs 2025 2026 hybrid return to office",
      _ => "US IT recruitment job market news"
    };

    var prompt = $$"""
      Search the web for the latest US IT recruitment and job market news about: {{query}}
      Category: {{category}}

      Return a JSON array of 6-10 recent news items:
      [{"headline":"...","source":"...","summary":"2-3 sentence summary","date":"YYYY-MM-DD","url":"https://...","category":"{{category}}"}]

      Focus on articles from the last 30 days when possible.
      """;

    var system = "You are a recruitment news aggregator. Use web search for current news. Return only valid JSON array.";
    var raw = await claude.CompleteWithWebSearchAsync(system, prompt, ct);

    try
    {
      return ParseJsonArray<NewsItem>(raw);
    }
    catch
    {
      return [];
    }
  }

  private static List<T> ParseJsonArray<T>(string raw)
  {
    raw = raw.Trim();
    var match = Regex.Match(raw, @"\[[\s\S]*\]");
    if (match.Success) raw = match.Value;
    return JsonSerializer.Deserialize<List<T>>(raw, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
  }

  private record JobListingDto(string Company, string Title, string Location, string EmploymentType, string ApplyUrl, string? Description);
}
