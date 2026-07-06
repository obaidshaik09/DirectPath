export interface CandidateProfile {
  name?: string
  role?: string
  employmentType?: string
  experienceLevel?: string
  remotePreference?: string
  location?: string
  workAuthorization?: string
}

export function getProfile(): CandidateProfile | null {
  const raw = localStorage.getItem('directpath_profile')
  return raw ? JSON.parse(raw) : null
}

async function apiFetch<T>(path: string, body?: unknown): Promise<T> {
  const profile = getProfile()
  const payload = body && typeof body === 'object' ? { ...body, profile } : body
  const res = await fetch(path, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload ?? {}),
  })
  if (!res.ok) {
    const text = await res.text()
    throw new Error(text || `Request failed: ${res.status}`)
  }
  return res.json()
}

async function apiGet<T>(path: string): Promise<T> {
  const res = await fetch(path)
  if (!res.ok) throw new Error(`Request failed: ${res.status}`)
  return res.json()
}

export const api = {
  chat: (message: string, history: { role: string; content: string }[]) =>
    apiFetch<{ reply: string; usedRag: boolean; sources?: string[] }>('/api/chat', { message, history }),

  buildProfile: (background: string) =>
    apiFetch<{ summary: string; skills: string; experience: string; education: string; certifications: string; benchPitch: string }>(
      '/api/profile/build', { background }
    ),

  optimizeResume: (resumeText: string, jobDescription: string) =>
    apiFetch<{ optimizedResume: string; keywordAnalysis: { keyword: string; action: string; reason: string }[]; summary: string }>(
      '/api/resume/optimize', { resumeText, jobDescription }
    ),

  searchJobs: (params: { skills?: string; location?: string; employmentType?: string; visaRequirement?: string }) =>
    apiFetch<{ jobs: { company: string; title: string; location: string; employmentType: string; applyUrl: string; description?: string }[] }>(
      '/api/jobs/search', params
    ),

  calculateSalary: (params: { role: string; location: string; experienceLevel: string; employmentType: string }) =>
    apiFetch<{ hourlyMin: number; hourlyMax: number; salaryMin: number; salaryMax: number; context: string; negotiationTips: string[] }>(
      '/api/salary/calculate', params
    ),

  generateOutreach: (params: { targetCompany: string; role: string; candidateBackground: string }) =>
    apiFetch<{ connectionMessage: string; followUpMessages: string[] }>('/api/outreach/generate', params),

  interviewPrep: (role: string, company: string) =>
    apiFetch<{ questions: { question: string; type: string; starCoaching: string }[] }>(
      '/api/interview/prep', { role, company }
    ),

  newsFeed: () =>
    apiFetch<{ items: NewsItem[]; cachedAt: string }>('/api/news/feed'),

  newsSearch: (keyword: string) =>
    apiFetch<NewsItem[]>('/api/news/search', { keyword }),

  ingest: (title: string, text: string) =>
    apiFetch<{ documentId: number; chunksCreated: number }>('/api/ingest', { title, text, source: 'manual' }),

  ingestUrl: (url: string) =>
    apiFetch<{ documentId: number; chunksCreated: number }>('/api/ingest-url', { url }),

  adminStats: () => apiGet<{ documentCount: number; chunkCount: number }>('/api/admin/stats'),

  autoPopulate: () => apiFetch<{ documentCount: number; chunkCount: number }>('/api/admin/auto-populate'),
}

export interface NewsItem {
  headline: string
  source: string
  summary: string
  date: string
  url: string
  category: string
}
