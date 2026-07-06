import { useState } from 'react'
import { api, getProfile } from '../api/client'
import { Loader2 } from 'lucide-react'

export default function SalaryCalculator() {
  const profile = getProfile()
  const [role, setRole] = useState(profile?.role ?? '')
  const [location, setLocation] = useState(profile?.location ?? '')
  const [experienceLevel, setExperienceLevel] = useState(profile?.experienceLevel ?? '3-5 years')
  const [employmentType, setEmploymentType] = useState('Both')
  const [loading, setLoading] = useState(false)
  const [result, setResult] = useState<Awaited<ReturnType<typeof api.calculateSalary>> | null>(null)
  const [error, setError] = useState('')

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      const res = await api.calculateSalary({ role, location, experienceLevel, employmentType })
      setResult(res)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Calculation failed')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-4xl mx-auto px-4 py-12">
      <h1 className="text-3xl font-bold mb-2">Market Rate Calculator</h1>
      <p className="text-gray-400 mb-8">Know your worth — get salary and contract rate ranges with negotiation tips.</p>

      <form onSubmit={handleSubmit} className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
        <input value={role} onChange={(e) => setRole(e.target.value)} placeholder="Role" required
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        <input value={location} onChange={(e) => setLocation(e.target.value)} placeholder="Location" required
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none" />
        <select value={experienceLevel} onChange={(e) => setExperienceLevel(e.target.value)}
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none">
          <option>0-2 years</option>
          <option>3-5 years</option>
          <option>6-10 years</option>
          <option>10+ years</option>
        </select>
        <select value={employmentType} onChange={(e) => setEmploymentType(e.target.value)}
          className="px-4 py-3 rounded-lg bg-gray-900 border border-gray-700 focus:border-emerald-500 focus:outline-none">
          <option>Full time</option>
          <option>Contract</option>
          <option>Both</option>
        </select>
        <button type="submit" disabled={loading}
          className="md:col-span-2 py-3 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 rounded-lg font-medium flex items-center justify-center gap-2">
          {loading && <Loader2 className="w-4 h-4 animate-spin" />}
          Calculate Market Rate
        </button>
      </form>

      {error && <p className="text-red-400 mb-4">{error}</p>}

      {result && (
        <div className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
              <h3 className="text-sm text-gray-500 mb-2">Contract Rate (Hourly)</h3>
              <p className="text-3xl font-bold text-emerald-400">${result.hourlyMin} – ${result.hourlyMax}<span className="text-lg text-gray-500">/hr</span></p>
            </div>
            <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
              <h3 className="text-sm text-gray-500 mb-2">Full-Time Salary (Annual)</h3>
              <p className="text-3xl font-bold text-emerald-400">${result.salaryMin.toLocaleString()} – ${result.salaryMax.toLocaleString()}</p>
            </div>
          </div>
          <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
            <h3 className="text-lg font-semibold mb-3">Market Context</h3>
            <p className="text-gray-300 leading-relaxed">{result.context}</p>
          </div>
          <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
            <h3 className="text-lg font-semibold mb-3">Negotiation Tips</h3>
            <ul className="space-y-2">
              {result.negotiationTips.map((tip, i) => (
                <li key={i} className="text-gray-300 flex gap-2">
                  <span className="text-emerald-400">•</span> {tip}
                </li>
              ))}
            </ul>
          </div>
        </div>
      )}
    </div>
  )
}
