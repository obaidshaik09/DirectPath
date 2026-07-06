using Pgvector;

namespace DirectPath.Api.Models;

public class Document
{
  public int Id { get; set; }
  public string Title { get; set; } = "";
  public string Source { get; set; } = "";
  public string? Url { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public List<Chunk> Chunks { get; set; } = [];
}

public class Chunk
{
  public int Id { get; set; }
  public int DocumentId { get; set; }
  public Document Document { get; set; } = null!;
  public string Content { get; set; } = "";
  public Vector? Embedding { get; set; }
  public int ChunkIndex { get; set; }
}

public class NewsCacheEntry
{
  public int Id { get; set; }
  public string Category { get; set; } = "";
  public string ItemsJson { get; set; } = "[]";
  public DateTime CachedAt { get; set; } = DateTime.UtcNow;
}
