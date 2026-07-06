using DirectPath.Api.Models;

namespace DirectPath.Api.Services;

public static class ProfileHelper
{
  public static string ToContext(CandidateProfile? profile)
  {
    if (profile == null) return "";
    var parts = new List<string>();
    if (!string.IsNullOrWhiteSpace(profile.Name)) parts.Add($"Name: {profile.Name}");
    if (!string.IsNullOrWhiteSpace(profile.Role)) parts.Add($"Role/Skills: {profile.Role}");
    if (!string.IsNullOrWhiteSpace(profile.EmploymentType)) parts.Add($"Employment preference: {profile.EmploymentType}");
    if (!string.IsNullOrWhiteSpace(profile.ExperienceLevel)) parts.Add($"Experience: {profile.ExperienceLevel}");
    if (!string.IsNullOrWhiteSpace(profile.RemotePreference)) parts.Add($"Work mode: {profile.RemotePreference}");
    if (!string.IsNullOrWhiteSpace(profile.Location)) parts.Add($"Target location: {profile.Location}");
    if (!string.IsNullOrWhiteSpace(profile.WorkAuthorization)) parts.Add($"Work authorization: {profile.WorkAuthorization}");
    return parts.Count == 0 ? "" : "Candidate profile:\n" + string.Join("\n", parts);
  }
}
