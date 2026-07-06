namespace DirectPath.Api.Models;

public record CandidateProfile(
  string? Name,
  string? Role,
  string? EmploymentType,
  string? ExperienceLevel,
  string? RemotePreference,
  string? Location,
  string? WorkAuthorization
);

public record ChatRequest(string Message, List<ChatMessage>? History, CandidateProfile? Profile);
public record ChatMessage(string Role, string Content);
public record ChatResponse(string Reply, bool UsedRag, List<string>? Sources);

public record ProfileBuildRequest(string Background, CandidateProfile? Profile);

public record ResumeOptimizeRequest(string ResumeText, string JobDescription, CandidateProfile? Profile);
public record KeywordChange(string Keyword, string Action, string Reason);

public record JobSearchRequest(
  string? Skills,
  string? Location,
  string? EmploymentType,
  string? VisaRequirement,
  CandidateProfile? Profile
);
public record JobListing(string Company, string Title, string Location, string EmploymentType, string ApplyUrl, string? Description);
public record JobSearchResponse(List<JobListing> Jobs);

public record SalaryCalculateRequest(
  string Role,
  string Location,
  string ExperienceLevel,
  string EmploymentType,
  CandidateProfile? Profile
);

public record OutreachGenerateRequest(
  string TargetCompany,
  string Role,
  string CandidateBackground,
  CandidateProfile? Profile
);

public record InterviewPrepRequest(string Role, string Company, CandidateProfile? Profile);
public record InterviewQuestion(string Question, string Type, string StarCoaching);

public record NewsItem(string Headline, string Source, string Summary, string Date, string Url, string Category);
public record NewsFeedResponse(List<NewsItem> Items, DateTime CachedAt);
public record NewsSearchRequest(string Keyword);

public record IngestRequest(string Title, string Text, string? Source);
public record IngestUrlRequest(string Url, string? Title);
public record IngestResponse(int DocumentId, int ChunksCreated);

public record AdminStatsResponse(int DocumentCount, int ChunkCount);
