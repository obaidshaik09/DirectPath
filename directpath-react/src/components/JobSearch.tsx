import { useState } from 'react'
import { api, getProfile } from '../api/client'
import { Loader2, ExternalLink } from 'lucide-react'

export default function JobSearch() {
  const profile = getProfile()
  const [skills, setSkills] = useState(profile?.role ?? '')
  const [location, setLocation] = useState(profile?.location ?? '')
  const [employmentType, setEmploymentType] = useState(profile?.employmentType ?? 'Both')
  const [visaRequirement, setVisaRequirement] = useState(profile?.workAuthorization ?? '')
  const [loading, setLoading] = useState(false)
  const [jobs, setJobs] = useState<Awaited<ReturnType<typeof api.searchJobs>>['jobs']>([])
  const [error, setError] = useState('')

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      const res = await api.searchJobs({ skills, location, employmentType, visaRequirement })
      setJobs(res.jobs)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Search failed')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-5xl mx-auto px-4 py-12">
      <h1 className="text-3xl font-bold mb-2">Job Search</h1>
      <p className="text-gray-400 mb-8">Search real full-time and contract IT roles matching your skills.</p>

      <form onSubmit={handleSearch} className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
        <input value={skills} onChange={(e) => setSkills(e.target.value)} placeholder="Skills / Role" required
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        <input value={location} onChange={(e) => setLocation(e.target.value)} placeholder="Location"
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        <select value={employmentType} onChange={(e) => setEmploymentType(e.target.value)}
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none">
          <option>Full time</option>
          <option>Contract (C2C/1099)</option>
          <option>Both</option>
        </select>
        <input value={visaRequirement} onChange={(e) => setVisaRequirement(e.target.value)} placeholder="Visa / Work auth (optional)"
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        <button type="submit" disabled={loading}
          className="md:col-span-2 py-3 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 rounded-lg font-medium flex items-center justify-center gap-2">
          {loading && <Loader2 className="w-4 h-4 animate-spin" />}
          Search Jobs
        </button>
      </form>

      {error && <p className="text-red-400 mb-4">{error}</p>}

      <div className="space-y-4">
        {jobs.map((job, i) => (
          <div key={i} className="bg-gray-900 border border-gray-800 rounded-xl p-6 hover:border-gray-700 transition-colors">
            <div className="flex justify-between items-start gap-4">
              <div>
                <h3 className="text-lg font-semibold">{job.title}</h3>
                <p className="text-emerald-400">{job.company}</p>
                <div className="flex gap-3 mt-2 text-sm text-gray-500">
                  <span>{job.location}</span>
                  <span>•</span>
                  <span>{job.employmentType}</span>
                </div>
                {job.description && <p className="mt-3 text-sm text-gray-400">{job.description}</p>}
              </div>
              <a href={job.applyUrl} target="_blank" rel="noopener noreferrer"
                className="flex-shrink-0 px-4 py-2 bg-emerald-600 hover:bg-emerald-500 rounded-lg text-sm font-medium flex items-center gap-1">
                Apply <ExternalLink className="w-3 h-3" />
              </a>
            </div>
          </div>
        ))}
        {!loading && jobs.length === 0 && (
          <p className="text-gray-500 text-center py-8">Search to find matching job postings.</p>
        )}
      </div>
    </div>
  )
}
