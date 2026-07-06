namespace DirectPath.Api.Config;

public class AppSettings
{
    public string OpenRouterApiKey { get; set; } = "";
    public string AnthropicApiKey { get; set; } = "";
    public string ConnectionString { get; set; } = "";
    public const double SimilarityThreshold = 0.5;
    public const int EmbeddingDimensions = 1536;
    public const string EmbeddingModel = "openai/text-embedding-3-small";
    public const string ClaudeModel = "claude-sonnet-4-6";
}
