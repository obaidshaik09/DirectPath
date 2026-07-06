import { useState } from 'react'
import { api, getProfile } from '../api/client'
import { Loader2, Copy, Check } from 'lucide-react'

export default function OutreachGenerator() {
  const profile = getProfile()
  const [targetCompany, setTargetCompany] = useState('')
  const [role, setRole] = useState('')
  const [candidateBackground, setCandidateBackground] = useState(
    profile ? `${profile.role ?? ''} with ${profile.experienceLevel ?? ''} experience` : ''
  )
  const [loading, setLoading] = useState(false)
  const [result, setResult] = useState<Awaited<ReturnType<typeof api.generateOutreach>> | null>(null)
  const [error, setError] = useState('')
  const [copied, setCopied] = useState<number | null>(null)

  const copy = (text: string, idx: number) => {
    navigator.clipboard.writeText(text)
    setCopied(idx)
    setTimeout(() => setCopied(null), 2000)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      const res = await api.generateOutreach({ targetCompany, role, candidateBackground })
      setResult(res)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Generation failed')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-4xl mx-auto px-4 py-12">
      <h1 className="text-3xl font-bold mb-2">Outreach Generator</h1>
      <p className="text-gray-400 mb-8">Generate personalized LinkedIn messages to reach hiring managers directly.</p>

      <form onSubmit={handleSubmit} className="space-y-4 mb-8">
        <input value={targetCompany} onChange={(e) => setTargetCompany(e.target.value)} placeholder="Target Company" required
          className="w-full px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        <input value={role} onChange={(e) => setRole(e.target.value)} placeholder="Role" required
          className="w-full px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        <textarea value={candidateBackground} onChange={(e) => setCandidateBackground(e.target.value)} placeholder="Your background summary" rows={4} required
          className="w-full px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none resize-none" />
        <button type="submit" disabled={loading}
          className="px-6 py-3 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 rounded-lg font-medium flex items-center gap-2">
          {loading && <Loader2 className="w-4 h-4 animate-spin" />}
          Generate Messages
        </button>
      </form>

      {error && <p className="text-red-400 mb-4">{error}</p>}

      {result && (
        <div className="space-y-4">
          <MessageCard title="Connection Request" message={result.connectionMessage} index={0} copied={copied} onCopy={copy} />
          {result.followUpMessages.map((msg, i) => (
            <MessageCard key={i} title={`Follow-up ${i + 1} (Day ${[3, 7, 14][i]})`} message={msg} index={i + 1} copied={copied} onCopy={copy} />
          ))}
        </div>
      )}
    </div>
  )
}

function MessageCard({ title, message, index, copied, onCopy }: {
  title: string; message: string; index: number; copied: number | null
  onCopy: (text: string, idx: number) => void
}) {
  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
      <div className="flex justify-between items-center mb-3">
        <h3 className="font-semibold text-emerald-400">{title}</h3>
        <button onClick={() => onCopy(message, index)} className="text-gray-500 hover:text-gray-300">
          {copied === index ? <Check className="w-4 h-4 text-emerald-400" /> : <Copy className="w-4 h-4" />}
        </button>
      </div>
      <p className="text-gray-300 leading-relaxed">{message}</p>
    </div>
  )
}
