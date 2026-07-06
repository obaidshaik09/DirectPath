using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DirectPath.Api.Data;

public static class DatabaseInitializer
{
  public static async Task EnsureDatabaseAsync(string masterConnectionString, string databaseName)
  {
    await using var conn = new NpgsqlConnection(masterConnectionString);
    await conn.OpenAsync();
    await using var check = conn.CreateCommand();
    check.CommandText = "SELECT 1 FROM pg_database WHERE datname = @name";
    check.Parameters.AddWithValue("name", databaseName);
    var exists = await check.ExecuteScalarAsync();
    if (exists == null)
    {
      await using var create = conn.CreateCommand();
      create.CommandText = $"CREATE DATABASE \"{databaseName}\"";
      await create.ExecuteNonQueryAsync();
    }
  }

  public static async Task EnsureVectorExtensionAsync(string connectionString)
  {
    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();
    await using var cmd = conn.CreateCommand();
    cmd.CommandText = "CREATE EXTENSION IF NOT EXISTS vector";
    await cmd.ExecuteNonQueryAsync();
  }

  public static async Task EnsureSchemaAsync(DirectPathDbContext db)
  {
    var conn = db.Database.GetDbConnection();
    await conn.OpenAsync();
    await using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT to_regclass('public.documents')::text";
    var exists = await cmd.ExecuteScalarAsync() as string;

    if (string.IsNullOrEmpty(exists))
    {
      try { await db.Database.EnsureDeletedAsync(); } catch { /* ignore */ }
      await db.Database.EnsureCreatedAsync();
    }
    await conn.CloseAsync();
  }
}
