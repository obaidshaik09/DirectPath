using DirectPath.Api.Config;
using DirectPath.Api.Data;
using DirectPath.Api.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace DirectPath.Api.Services;

public class RagService(DirectPathDbContext db, EmbeddingService embeddings)
{
  public static List<string> ChunkText(string text, int chunkSize = 500, int overlap = 100)
  {
    var chunks = new List<string>();
    if (string.IsNullOrWhiteSpace(text)) return chunks;

    var normalized = text.Replace("\r\n", "\n").Trim();
    if (normalized.Length <= chunkSize)
    {
      chunks.Add(normalized);
      return chunks;
    }

    var start = 0;
    while (start < normalized.Length)
    {
      var end = Math.Min(start + chunkSize, normalized.Length);
      if (end < normalized.Length)
      {
        var breakAt = normalized.LastIndexOf('\n', end - 1, Math.Min(100, end - start));
        if (breakAt > start) end = breakAt + 1;
        else
        {
          breakAt = normalized.LastIndexOf(' ', end - 1, Math.Min(80, end - start));
          if (breakAt > start) end = breakAt + 1;
        }
      }
      chunks.Add(normalized[start..end].Trim());
      if (end >= normalized.Length) break;
      start = Math.Max(end - overlap, start + 1);
    }
    return chunks.Where(c => c.Length > 20).ToList();
  }

  public async Task<IngestResponse> IngestAsync(string title, string text, string source, string? url = null, CancellationToken ct = default)
  {
    var doc = new Document { Title = title, Source = source, Url = url };
    db.Documents.Add(doc);
    await db.SaveChangesAsync(ct);

    var texts = ChunkText(text);
    var vectors = await embeddings.EmbedBatchAsync(texts, ct);

    for (var i = 0; i < texts.Count; i++)
    {
      db.Chunks.Add(new Chunk
      {
        DocumentId = doc.Id,
        Content = texts[i],
        Embedding = vectors[i],
        ChunkIndex = i
      });
    }
    await db.SaveChangesAsync(ct);
    return new IngestResponse(doc.Id, texts.Count);
  }

  public async Task<List<(Chunk Chunk, double Similarity)>> SearchAsync(string query, int limit = 8, CancellationToken ct = default)
  {
    var queryVec = await embeddings.EmbedAsync(query, ct);
    var all = await db.Chunks.Where(c => c.Embedding != null).ToListAsync(ct);

    return all
      .Select(c => (Chunk: c, Similarity: 1.0 - CosineDistance(c.Embedding!, queryVec)))
      .Where(x => x.Similarity >= AppSettings.SimilarityThreshold)
      .OrderByDescending(x => x.Similarity)
      .Take(limit)
      .ToList();
  }

  public string BuildContext(List<(Chunk Chunk, double Similarity)> results)
  {
    if (results.Count == 0) return "";
    return string.Join("\n\n---\n\n", results.Select(r =>
      $"[Relevance: {r.Similarity:P0}]\n{r.Chunk.Content}"));
  }

  private static double CosineDistance(Vector a, Vector b)
  {
    var av = a.ToArray();
    var bv = b.ToArray();
    double dot = 0, magA = 0, magB = 0;
    for (var i = 0; i < av.Length; i++)
    {
      dot += av[i] * bv[i];
      magA += av[i] * av[i];
      magB += bv[i] * bv[i];
    }
    if (magA == 0 || magB == 0) return 1.0;
    return 1.0 - dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
  }
}
