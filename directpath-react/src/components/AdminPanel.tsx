import { useState, useEffect } from 'react'
import { api } from '../api/client'
import { Loader2, Database, RefreshCw } from 'lucide-react'

export default function AdminPanel() {
  const [stats, setStats] = useState({ documentCount: 0, chunkCount: 0 })
  const [loading, setLoading] = useState(true)
  const [populating, setPopulating] = useState(false)
  const [ingesting, setIngesting] = useState(false)
  const [url, setUrl] = useState('')
  const [textTitle, setTextTitle] = useState('')
  const [textContent, setTextContent] = useState('')
  const [message, setMessage] = useState('')
  const [error, setError] = useState('')

  const loadStats = async () => {
    try {
      const res = await api.adminStats()
      setStats(res)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load stats')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { loadStats() }, [])

  const handleAutoPopulate = async () => {
    setPopulating(true)
    setMessage('')
    setError('')
    try {
      const res = await api.autoPopulate()
      setStats(res)
      setMessage(`Knowledge base populated: ${res.chunkCount} chunks across ${res.documentCount} documents.`)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Auto-populate failed')
    } finally {
      setPopulating(false)
    }
  }

  const handleIngestUrl = async (e: React.FormEvent) => {
    e.preventDefault()
    setIngesting(true)
    setMessage('')
    try {
      const res = await api.ingestUrl(url)
      setMessage(`Ingested URL: ${res.chunksCreated} chunks created.`)
      setUrl('')
      await loadStats()
    } catch (err) {
      setError(err instanceof Error ? err.message : 'URL ingest failed')
    } finally {
      setIngesting(false)
    }
  }

  const handleIngestText = async (e: React.FormEvent) => {
    e.preventDefault()
    setIngesting(true)
    setMessage('')
    try {
      const res = await api.ingest(textTitle, textContent)
      setMessage(`Ingested text: ${res.chunksCreated} chunks created.`)
      setTextTitle('')
      setTextContent('')
      await loadStats()
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Text ingest failed')
    } finally {
      setIngesting(false)
    }
  }

  return (
    <div className="max-w-4xl mx-auto px-4 py-12">
      <h1 className="text-3xl font-bold mb-2">Admin Panel</h1>
      <p className="text-gray-400 mb-8">Manage the RAG knowledge base.</p>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
        <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
          <div className="flex items-center gap-2 mb-2">
            <Database className="w-5 h-5 text-emerald-400" />
            <h3 className="font-semibold">Documents</h3>
          </div>
          <p className="text-4xl font-bold">{loading ? '...' : stats.documentCount}</p>
        </div>
        <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
          <div className="flex items-center gap-2 mb-2">
            <Database className="w-5 h-5 text-emerald-400" />
            <h3 className="font-semibold">Chunks</h3>
          </div>
          <p className="text-4xl font-bold">{loading ? '...' : stats.chunkCount}</p>
        </div>
      </div>

      <button onClick={handleAutoPopulate} disabled={populating}
        className="w-full mb-8 py-4 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 rounded-xl font-medium flex items-center justify-center gap-2">
        {populating ? <Loader2 className="w-5 h-5 animate-spin" /> : <RefreshCw className="w-5 h-5" />}
        {populating ? 'Populating Knowledge Base...' : 'Auto-populate Knowledge Base (500+ chunks)'}
      </button>

      {message && <p className="text-emerald-400 mb-4">{message}</p>}
      {error && <p className="text-red-400 mb-4">{error}</p>}

      <div className="space-y-8">
        <form onSubmit={handleIngestUrl} className="bg-gray-900 border border-gray-800 rounded-xl p-6 space-y-4">
          <h3 className="font-semibold">Ingest URL</h3>
          <input value={url} onChange={(e) => setUrl(e.target.value)} placeholder="https://..." required
            className="w-full px-4 py-3 rounded-lg bg-gray-800 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
          <button type="submit" disabled={ingesting}
            className="px-6 py-2 bg-gray-700 hover:bg-gray-600 disabled:opacity-50 rounded-lg text-sm font-medium">
            Ingest URL
          </button>
        </form>

        <form onSubmit={handleIngestText} className="bg-gray-900 border border-gray-800 rounded-xl p-6 space-y-4">
          <h3 className="font-semibold">Ingest Text</h3>
          <input value={textTitle} onChange={(e) => setTextTitle(e.target.value)} placeholder="Title" required
            className="w-full px-4 py-3 rounded-lg bg-gray-800 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
          <textarea value={textContent} onChange={(e) => setTextContent(e.target.value)} placeholder="Text content..." rows={6} required
            className="w-full px-4 py-3 rounded-lg bg-gray-800 border border-gray-700 focus:border-emerald-500 focus:outline-none resize-none" />
          <button type="submit" disabled={ingesting}
            className="px-6 py-2 bg-gray-700 hover:bg-gray-600 disabled:opacity-50 rounded-lg text-sm font-medium">
            Ingest Text
          </button>
        </form>
      </div>
    </div>
  )
}
