using DirectPath.Api.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace DirectPath.Api.Data;

public class DirectPathDbContext(DbContextOptions<DirectPathDbContext> options) : DbContext(options)
{
  public DbSet<Document> Documents => Set<Document>();
  public DbSet<Chunk> Chunks => Set<Chunk>();
  public DbSet<NewsCacheEntry> NewsCache => Set<NewsCacheEntry>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresExtension("vector");

    modelBuilder.Entity<Document>(e =>
    {
      e.ToTable("documents");
      e.HasKey(d => d.Id);
      e.Property(d => d.Title).HasMaxLength(500);
      e.Property(d => d.Source).HasMaxLength(200);
      e.Property(d => d.Url).HasMaxLength(2000);
      e.Property(d => d.CreatedAt).HasColumnName("created_at");
    });

    modelBuilder.Entity<Chunk>(e =>
    {
      e.ToTable("chunks");
      e.HasKey(c => c.Id);
      e.Property(c => c.Content).IsRequired();
      e.Property(c => c.Embedding).HasColumnType("vector(1536)");
      e.Property(c => c.ChunkIndex).HasColumnName("chunk_index");
      e.HasOne(c => c.Document).WithMany(d => d.Chunks).HasForeignKey(c => c.DocumentId);
    });

    modelBuilder.Entity<NewsCacheEntry>(e =>
    {
      e.ToTable("news_cache");
      e.HasKey(n => n.Id);
      e.Property(n => n.Category).HasMaxLength(100);
      e.Property(n => n.ItemsJson).HasColumnName("items_json");
      e.Property(n => n.CachedAt).HasColumnName("cached_at");
    });
  }
}
