import { useState } from 'react'
import { api } from '../api/client'
import { Loader2, ChevronDown, ChevronUp } from 'lucide-react'

export default function InterviewPrep() {
  const [role, setRole] = useState('')
  const [company, setCompany] = useState('')
  const [loading, setLoading] = useState(false)
  const [questions, setQuestions] = useState<Awaited<ReturnType<typeof api.interviewPrep>>['questions']>([])
  const [error, setError] = useState('')
  const [expanded, setExpanded] = useState<number | null>(null)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      const res = await api.interviewPrep(role, company)
      setQuestions(res.questions)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to generate questions')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-4xl mx-auto px-4 py-12">
      <h1 className="text-3xl font-bold mb-2">Interview Prep</h1>
      <p className="text-gray-400 mb-8">Get tailored technical and behavioral questions with STAR method coaching.</p>

      <form onSubmit={handleSubmit} className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
        <input value={role} onChange={(e) => setRole(e.target.value)} placeholder="Role" required
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        <input value={company} onChange={(e) => setCompany(e.target.value)} placeholder="Company" required
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        <button type="submit" disabled={loading}
          className="md:col-span-2 py-3 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 rounded-lg font-medium flex items-center justify-center gap-2">
          {loading && <Loader2 className="w-4 h-4 animate-spin" />}
          Generate Questions
        </button>
      </form>

      {error && <p className="text-red-400 mb-4">{error}</p>}

      <div className="space-y-3">
        {questions.map((q, i) => (
          <div key={i} className="bg-gray-900 border border-gray-800 rounded-xl overflow-hidden">
            <button
              onClick={() => setExpanded(expanded === i ? null : i)}
              className="w-full px-6 py-4 flex justify-between items-center text-left hover:bg-gray-800/50"
            >
              <div>
                <span className={`text-xs px-2 py-0.5 rounded mr-2 ${
                  q.type === 'technical' ? 'bg-blue-500/20 text-blue-400' : 'bg-purple-500/20 text-purple-400'
                }`}>{q.type}</span>
                <span className="font-medium">{q.question}</span>
              </div>
              {expanded === i ? <ChevronUp className="w-4 h-4 text-gray-500" /> : <ChevronDown className="w-4 h-4 text-gray-500" />}
            </button>
            {expanded === i && (
              <div className="px-6 pb-4 border-t border-gray-800">
                <h4 className="text-sm font-medium text-emerald-400 mt-3 mb-2">STAR Method Coaching</h4>
                <p className="text-gray-400 text-sm leading-relaxed">{q.starCoaching}</p>
              </div>
            )}
          </div>
        ))}
      </div>
    </div>
  )
}
