using DirectPath.Api.Data;
using DirectPath.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectPath.Api.Services;

public class AutoPopulateService(RagService rag, ClaudeService claude, DirectPathDbContext db)
{
  private static readonly string[] Topics =
  [
    "US IT job market overview 2025 2026 trends",
    "contract vs full time employment IT professionals differences",
    "C2C vs W2 vs 1099 contractor employment explained",
    "bench sales recruiting model how it works red flags",
    "ATS applicant tracking systems how to optimize resume",
    "recruiter scripts and techniques presenting candidates",
    "salary negotiation strategies IT professionals",
    "LinkedIn outreach messages hiring managers templates",
    "US visa work authorization H1B OPT GC basics IT workers",
    "technical behavioral interview preparation STAR method",
    "top IT job portals Dice Indeed LinkedIn FlexJobs",
    "recruiter red flags candidates should avoid",
    "contractor rate negotiation hourly billing tips",
    "current US tech hiring trends layoffs hiring freezes",
    "staffing agency vs direct hire pros cons",
    "resume keywords ATS matching software engineering",
    "networking strategies IT job search without recruiter",
    "remote hybrid onsite work arrangements IT jobs",
    "Java Python cloud AWS DevOps in-demand skills",
    "immigration sponsorship employer requirements tech"
  ];

  public async Task<int> PopulateAsync(CancellationToken ct = default)
  {
    var existing = await db.Chunks.CountAsync(ct);
    if (existing >= 500) return existing;

    var totalChunks = existing;

    // Seed fallback first for reliable baseline coverage
    var seedIndex = 0;
    while (totalChunks < 500 && seedIndex < RecruitmentSeedContent.Count)
    {
      var batch = new System.Text.StringBuilder();
      for (var i = 0; i < 8 && seedIndex < RecruitmentSeedContent.Count; i++, seedIndex++)
        batch.AppendLine(RecruitmentSeedContent.GetChunk(seedIndex));
      var text = batch.ToString().Trim();
      if (string.IsNullOrEmpty(text)) break;
      var result = await rag.IngestAsync($"Recruitment Knowledge Batch {seedIndex}", text, "seed", null, ct);
      totalChunks += result.ChunksCreated;
    }

    foreach (var topic in Topics)
    {
      if (totalChunks >= 500) break;

      try
      {
        var system = "You are a recruitment industry expert. Write detailed, factual educational content for IT job seekers.";
        var prompt = $"""
          Research and write comprehensive educational content about: {topic}
          
          Write 2500-4000 words covering practical insider knowledge that recruiters know but candidates often don't.
          Include specific examples, terminology, and actionable advice.
          Do not use markdown headers with # symbols — use plain paragraphs separated by blank lines.
          """;

        var content = await claude.CompleteWithWebSearchAsync(system, prompt, ct);
        if (string.IsNullOrWhiteSpace(content)) continue;

        var result = await rag.IngestAsync(topic, content, "auto-populate", null, ct);
        totalChunks += result.ChunksCreated;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Auto-populate warning for '{topic}': {ex.Message}");
      }
    }

    return totalChunks;
  }
}
