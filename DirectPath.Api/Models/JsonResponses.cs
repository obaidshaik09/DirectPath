using System.Text.Json.Serialization;

namespace DirectPath.Api.Models;

public record ProfileBuildResponse(
  [property: JsonPropertyName("summary")] string Summary,
  [property: JsonPropertyName("skills")] string Skills,
  [property: JsonPropertyName("experience")] string Experience,
  [property: JsonPropertyName("education")] string Education,
  [property: JsonPropertyName("certifications")] string Certifications,
  [property: JsonPropertyName("benchPitch")] string BenchPitch
);

public record ResumeOptimizeResponse(
  [property: JsonPropertyName("optimizedResume")] string OptimizedResume,
  [property: JsonPropertyName("keywordAnalysis")] List<KeywordChange> KeywordAnalysis,
  [property: JsonPropertyName("summary")] string Summary
);

public record SalaryCalculateResponse(
  [property: JsonPropertyName("hourlyMin")] decimal HourlyMin,
  [property: JsonPropertyName("hourlyMax")] decimal HourlyMax,
  [property: JsonPropertyName("salaryMin")] decimal SalaryMin,
  [property: JsonPropertyName("salaryMax")] decimal SalaryMax,
  [property: JsonPropertyName("context")] string Context,
  [property: JsonPropertyName("negotiationTips")] List<string> NegotiationTips
);

public record OutreachGenerateResponse(
  [property: JsonPropertyName("connectionMessage")] string ConnectionMessage,
  [property: JsonPropertyName("followUpMessages")] List<string> FollowUpMessages
);

public record InterviewPrepResponse(
  [property: JsonPropertyName("questions")] List<InterviewQuestion> Questions
);
