import { useState, useRef } from 'react'
import { api } from '../api/client'
import { Loader2, Upload } from 'lucide-react'

export default function ResumeOptimizer() {
  const [resumeText, setResumeText] = useState('')
  const [jobDescription, setJobDescription] = useState('')
  const [loading, setLoading] = useState(false)
  const [result, setResult] = useState<Awaited<ReturnType<typeof api.optimizeResume>> | null>(null)
  const [error, setError] = useState('')
  const fileRef = useRef<HTMLInputElement>(null)

  const handleFile = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0]
    if (!file) return
    const reader = new FileReader()
    reader.onload = () => setResumeText(reader.result as string)
    reader.readAsText(file)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      const res = await api.optimizeResume(resumeText, jobDescription)
      setResult(res)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to optimize resume')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-6xl mx-auto px-4 py-12">
      <h1 className="text-3xl font-bold mb-2">Resume Optimizer</h1>
      <p className="text-gray-400 mb-8">Paste your resume and a job description to get an ATS-optimized version with keyword analysis.</p>

      <form onSubmit={handleSubmit} className="space-y-4 mb-8">
        <div>
          <div className="flex justify-between items-center mb-2">
            <label className="text-sm font-medium">Resume</label>
            <button type="button" onClick={() => fileRef.current?.click()} className="text-sm text-emerald-400 flex items-center gap-1 hover:text-emerald-300">
              <Upload className="w-4 h-4" /> Upload file
            </button>
            <input ref={fileRef} type="file" accept=".txt,.md,.csv" className="hidden" onChange={handleFile} />
          </div>
          <textarea
            value={resumeText}
            onChange={(e) => setResumeText(e.target.value)}
            placeholder="Paste your resume text here..."
            rows={10}
            required
            className="w-full px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none resize-none font-mono text-sm"
          />
        </div>
        <div>
          <label className="text-sm font-medium mb-2 block">Job Description</label>
          <textarea
            value={jobDescription}
            onChange={(e) => setJobDescription(e.target.value)}
            placeholder="Paste the job description..."
            rows={8}
            required
            className="w-full px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none resize-none text-sm"
          />
        </div>
        <button type="submit" disabled={loading} className="px-6 py-3 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 rounded-lg font-medium flex items-center gap-2">
          {loading && <Loader2 className="w-4 h-4 animate-spin" />}
          Optimize Resume
        </button>
      </form>

      {error && <p className="text-red-400 mb-4">{error}</p>}

      {result && (
        <div className="space-y-6">
          <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
            <h3 className="text-lg font-semibold mb-3">Summary of Changes</h3>
            <p className="text-gray-300">{result.summary}</p>
          </div>

          <div className="bg-gray-900 border border-gray-800 rounded-xl p-6 overflow-x-auto">
            <h3 className="text-lg font-semibold mb-4">Keyword Analysis</h3>
            <table className="w-full text-sm">
              <thead>
                <tr className="text-left text-gray-500 border-b border-gray-800">
                  <th className="pb-2 pr-4">Keyword</th>
                  <th className="pb-2 pr-4">Action</th>
                  <th className="pb-2">Reason</th>
                </tr>
              </thead>
              <tbody>
                {result.keywordAnalysis.map((k, i) => (
                  <tr key={i} className="border-b border-gray-800/50">
                    <td className="py-2 pr-4 font-mono text-emerald-400">{k.keyword}</td>
                    <td className="py-2 pr-4">
                      <span className={`px-2 py-0.5 rounded text-xs ${
                        k.action === 'added' ? 'bg-green-500/20 text-green-400' :
                        k.action === 'removed' ? 'bg-red-500/20 text-red-400' :
                        'bg-yellow-500/20 text-yellow-400'
                      }`}>{k.action}</span>
                    </td>
                    <td className="py-2 text-gray-400">{k.reason}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className="bg-gray-900 border border-emerald-500/30 rounded-xl p-6">
            <h3 className="text-lg font-semibold mb-3 text-emerald-400">Optimized Resume</h3>
            <pre className="text-gray-300 whitespace-pre-wrap font-sans text-sm leading-relaxed">{result.optimizedResume}</pre>
          </div>
        </div>
      )}
    </div>
  )
}
