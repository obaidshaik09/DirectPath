using DirectPath.Api.Config;
using DirectPath.Api.Data;
using DirectPath.Api.Services;
using Microsoft.EntityFrameworkCore;

var settings = ConfigGate.LoadOrExit();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(settings);
builder.Services.AddHttpClient<EmbeddingService>();
builder.Services.AddHttpClient<ClaudeService>();
builder.Services.AddHttpClient<UrlScraperService>();
builder.Services.AddScoped<RagService>();
builder.Services.AddScoped<WebSearchService>();
builder.Services.AddScoped<NewsCacheService>();
builder.Services.AddScoped<AutoPopulateService>();

builder.Services.AddDbContext<DirectPathDbContext>(options =>
  options.UseNpgsql(settings.ConnectionString, o => o.UseVector()));

builder.Services.AddControllers()
  .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

builder.Services.AddCors(options =>
  options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:3002").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var masterConn = settings.ConnectionString.Replace("Database=directpath_db", "Database=postgres");
  await DatabaseInitializer.EnsureDatabaseAsync(masterConn, "directpath_db");
  await DatabaseInitializer.EnsureVectorExtensionAsync(settings.ConnectionString);

  var db = scope.ServiceProvider.GetRequiredService<DirectPathDbContext>();
  await DatabaseInitializer.EnsureSchemaAsync(db);

  try
  {
    await db.Database.ExecuteSqlRawAsync(
      "CREATE INDEX IF NOT EXISTS ix_chunks_embedding ON chunks USING hnsw (embedding vector_cosine_ops)");
  }
  catch (Exception ex)
  {
    Console.WriteLine($"HNSW index note: {ex.Message}");
  }
}

app.UseCors();
app.MapControllers();
app.Run("http://localhost:5000");
