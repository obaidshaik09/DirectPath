import { useState, useEffect, useCallback } from 'react'
import { api, type NewsItem } from '../api/client'
import { Loader2, ExternalLink, Search } from 'lucide-react'

const CATEGORIES = [
  { key: 'all', label: 'All' },
  { key: 'hiring_trends', label: 'Hiring Trends' },
  { key: 'layoffs', label: 'Layoffs' },
  { key: 'in_demand_skills', label: 'In-Demand Skills' },
  { key: 'salary_trends', label: 'Salary Trends' },
  { key: 'visa_updates', label: 'Visa Updates' },
  { key: 'remote_work', label: 'Remote Work' },
]

const REFRESH_MS = 6 * 60 * 60 * 1000

function isBreaking(dateStr: string) {
  const d = new Date(dateStr)
  return Date.now() - d.getTime() < 24 * 60 * 60 * 1000
}

export default function NewsPortal() {
  const [items, setItems] = useState<NewsItem[]>([])
  const [filtered, setFiltered] = useState<NewsItem[]>([])
  const [category, setCategory] = useState('all')
  const [keyword, setKeyword] = useState('')
  const [loading, setLoading] = useState(true)
  const [searching, setSearching] = useState(false)
  const [error, setError] = useState('')
  const [cachedAt, setCachedAt] = useState('')

  const loadFeed = useCallback(async () => {
    setLoading(true)
    setError('')
    try {
      const res = await api.newsFeed()
      setItems(res.items)
      setCachedAt(res.cachedAt)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load news')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => { loadFeed() }, [loadFeed])
  useEffect(() => {
    const interval = setInterval(loadFeed, REFRESH_MS)
    return () => clearInterval(interval)
  }, [loadFeed])

  useEffect(() => {
    let result = items
    if (category !== 'all') result = result.filter((i) => i.category === category)
    setFiltered(result)
  }, [items, category])

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!keyword.trim()) { loadFeed(); return }
    setSearching(true)
    try {
      const res = await api.newsSearch(keyword)
      setFiltered(res)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Search failed')
    } finally {
      setSearching(false)
    }
  }

  return (
    <div className="max-w-5xl mx-auto px-4 py-12">
      <h1 className="text-3xl font-bold mb-2">Recruitment News</h1>
      <p className="text-gray-400 mb-2">Latest US IT recruitment and job market news.</p>
      {cachedAt && <p className="text-xs text-gray-600 mb-6">Last updated: {new Date(cachedAt).toLocaleString()} • Auto-refreshes every 6 hours</p>}

      <form onSubmit={handleSearch} className="flex gap-2 mb-6">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500" />
          <input value={keyword} onChange={(e) => setKeyword(e.target.value)} placeholder="Search news by keyword..."
            className="w-full pl-10 pr-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        </div>
        <button type="submit" disabled={searching}
          className="px-6 py-3 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 rounded-lg font-medium">
          Search
        </button>
      </form>

      <div className="flex flex-wrap gap-2 mb-8">
        {CATEGORIES.map((c) => (
          <button key={c.key} onClick={() => { setCategory(c.key); setKeyword('') }}
            className={`px-4 py-2 rounded-full text-sm font-medium transition-colors ${
              category === c.key ? 'bg-emerald-600 text-white' : 'bg-gray-800 text-gray-400 hover:text-gray-200'
            }`}>
            {c.label}
          </button>
        ))}
      </div>

      {loading && <div className="flex justify-center py-12"><Loader2 className="w-8 h-8 animate-spin text-emerald-400" /></div>}
      {error && <p className="text-red-400 mb-4">{error}</p>}

      <div className="space-y-4">
        {filtered.map((item, i) => (
          <article key={i} className="bg-gray-900 border border-gray-800 rounded-xl p-6 hover:border-gray-700 transition-colors">
            <div className="flex items-start justify-between gap-4">
              <div>
                <div className="flex items-center gap-2 mb-2">
                  {isBreaking(item.date) && (
                    <span className="px-2 py-0.5 bg-red-500/20 text-red-400 text-xs font-bold rounded">Breaking</span>
                  )}
                  <span className="text-xs text-gray-500">{item.source} • {item.date}</span>
                </div>
                <h3 className="text-lg font-semibold mb-2">{item.headline}</h3>
                <p className="text-gray-400 text-sm leading-relaxed">{item.summary}</p>
              </div>
              <a href={item.url} target="_blank" rel="noopener noreferrer" className="flex-shrink-0 text-emerald-400 hover:text-emerald-300">
                <ExternalLink className="w-5 h-5" />
              </a>
            </div>
          </article>
        ))}
        {!loading && filtered.length === 0 && (
          <p className="text-gray-500 text-center py-8">No news items found.</p>
        )}
      </div>
    </div>
  )
}
