using System.Text.Json;
using DirectPath.Api.Data;
using DirectPath.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectPath.Api.Services;

public class NewsCacheService(DirectPathDbContext db, WebSearchService webSearch)
{
  private static readonly string[] Categories =
  [
    "hiring_trends", "layoffs", "in_demand_skills",
    "salary_trends", "visa_updates", "remote_work"
  ];

  private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(6);
  private static readonly SemaphoreSlim RefreshLock = new(1, 1);
  private List<NewsItem>? _memoryCache;
  private DateTime _memoryCachedAt = DateTime.MinValue;

  public async Task<NewsFeedResponse> GetFeedAsync(CancellationToken ct = default)
  {
    if (_memoryCache != null && DateTime.UtcNow - _memoryCachedAt < CacheTtl)
      return new NewsFeedResponse(_memoryCache, _memoryCachedAt);

    var dbEntry = await db.NewsCache.OrderByDescending(n => n.CachedAt).FirstOrDefaultAsync(ct);
    if (dbEntry != null && DateTime.UtcNow - dbEntry.CachedAt < CacheTtl)
    {
      var items = JsonSerializer.Deserialize<List<NewsItem>>(dbEntry.ItemsJson) ?? [];
      _memoryCache = items;
      _memoryCachedAt = dbEntry.CachedAt;
      return new NewsFeedResponse(items, dbEntry.CachedAt);
    }

    await RefreshLock.WaitAsync(ct);
    try
    {
      var allItems = new List<NewsItem>();
      foreach (var cat in Categories)
      {
        var items = await webSearch.FetchNewsByCategoryAsync(cat, ct);
        allItems.AddRange(items);
      }

      var json = JsonSerializer.Serialize(allItems);
      db.NewsCache.Add(new NewsCacheEntry { Category = "all", ItemsJson = json, CachedAt = DateTime.UtcNow });
      await db.SaveChangesAsync(ct);

      _memoryCache = allItems;
      _memoryCachedAt = DateTime.UtcNow;
      return new NewsFeedResponse(allItems, _memoryCachedAt);
    }
    finally
    {
      RefreshLock.Release();
    }
  }

  public async Task<List<NewsItem>> SearchAsync(string keyword, CancellationToken ct = default)
  {
    var feed = await GetFeedAsync(ct);
    var kw = keyword.ToLowerInvariant();
    return feed.Items.Where(i =>
      i.Headline.ToLowerInvariant().Contains(kw) ||
      i.Summary.ToLowerInvariant().Contains(kw) ||
      i.Category.ToLowerInvariant().Contains(kw)
    ).ToList();
  }
}
