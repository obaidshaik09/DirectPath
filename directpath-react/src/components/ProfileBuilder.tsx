import { useState } from 'react'
import { api } from '../api/client'
import { Loader2 } from 'lucide-react'

export default function ProfileBuilder() {
  const [background, setBackground] = useState('')
  const [loading, setLoading] = useState(false)
  const [result, setResult] = useState<Awaited<ReturnType<typeof api.buildProfile>> | null>(null)
  const [error, setError] = useState('')

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      const res = await api.buildProfile(background)
      setResult(res)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to build profile')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-4xl mx-auto px-4 py-12">
      <h1 className="text-3xl font-bold mb-2">Build My Profile</h1>
      <p className="text-gray-400 mb-8">Tell us about your background and we'll create a recruiter-style profile and bench pitch.</p>

      <form onSubmit={handleSubmit} className="space-y-4 mb-8">
        <textarea
          value={background}
          onChange={(e) => setBackground(e.target.value)}
          placeholder="Describe your experience, skills, certifications, projects, and career goals..."
          rows={8}
          required
          className="w-full px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none resize-none"
        />
        <button
          type="submit"
          disabled={loading}
          className="px-6 py-3 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 rounded-lg font-medium flex items-center gap-2"
        >
          {loading && <Loader2 className="w-4 h-4 animate-spin" />}
          Build Profile
        </button>
      </form>

      {error && <p className="text-red-400 mb-4">{error}</p>}

      {result && (
        <div className="space-y-6">
          <Section title="Summary" content={result.summary} />
          <Section title="Skills" content={result.skills} />
          <Section title="Experience" content={result.experience} />
          <Section title="Education" content={result.education} />
          <Section title="Certifications" content={result.certifications} />
          <div className="bg-emerald-500/10 border border-emerald-500/30 rounded-xl p-6">
            <h3 className="text-lg font-semibold text-emerald-400 mb-3">Bench Pitch</h3>
            <p className="text-gray-300 leading-relaxed">{result.benchPitch}</p>
          </div>
        </div>
      )}
    </div>
  )
}

function Section({ title, content }: { title: string; content: string }) {
  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
      <h3 className="text-lg font-semibold mb-3">{title}</h3>
      <p className="text-gray-300 whitespace-pre-wrap leading-relaxed">{content}</p>
    </div>
  )
}
