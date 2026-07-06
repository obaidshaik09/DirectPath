namespace DirectPath.Api.Config;

public static class ConfigGate
{
  private static readonly string[] RequiredKeys =
  [
    "OPENROUTER_API_KEY",
    "ANTHROPIC_API_KEY",
    "POSTGRES_PASSWORD"
  ];

  public static AppSettings LoadOrExit()
  {
    var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
    if (File.Exists(envPath))
      DotNetEnv.Env.Load(envPath);

    var missing = RequiredKeys
      .Where(k => string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(k)))
      .ToList();

    if (missing.Count > 0)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Error.WriteLine();
      Console.Error.WriteLine("╔══════════════════════════════════════════════════════════════╗");
      Console.Error.WriteLine("║  DirectPath.Api — Configuration Error                        ║");
      Console.Error.WriteLine("╠══════════════════════════════════════════════════════════════╣");
      Console.Error.WriteLine("║  Missing required environment variables in DirectPath.Api/.env ║");
      Console.Error.WriteLine("╚══════════════════════════════════════════════════════════════╝");
      Console.Error.WriteLine();
      foreach (var key in missing)
        Console.Error.WriteLine($"  ✗ {key}");
      Console.Error.WriteLine();
      Console.Error.WriteLine("  Copy .env.example to .env and fill in your API keys.");
      Console.Error.WriteLine("  The API will not start until all keys are configured.");
      Console.Error.WriteLine();
      Console.ResetColor();
      Environment.Exit(1);
    }

    var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")!;
    return new AppSettings
    {
      OpenRouterApiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")!,
      AnthropicApiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY")!,
      ConnectionString = $"Host=localhost;Port=5432;Database=directpath_db;Username=postgres;Password={password}"
    };
  }
}
