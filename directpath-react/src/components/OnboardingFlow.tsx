import { useState } from 'react'
import type { CandidateProfile } from '../api/client'

interface Props {
  onComplete: (profile: CandidateProfile) => void
  onSkip: () => void
}

const steps = [
  { key: 'name', label: 'What is your name?', type: 'text', placeholder: 'Your name' },
  { key: 'role', label: 'What is your current role or skillset?', type: 'text', placeholder: 'e.g. Senior Java Developer, AWS Cloud Engineer' },
  { key: 'employmentType', label: 'Looking for full time, contract (C2C/1099), or both?', type: 'select', options: ['Full time', 'Contract (C2C/1099)', 'Both'] },
  { key: 'experienceLevel', label: 'What is your experience level?', type: 'select', options: ['0-2 years', '3-5 years', '6-10 years', '10+ years'] },
  { key: 'remotePreference', label: 'Remote, hybrid, or onsite?', type: 'select', options: ['Remote', 'Hybrid', 'Onsite', 'Open to any'] },
  { key: 'location', label: 'What location or city are you targeting?', type: 'text', placeholder: 'e.g. Dallas, TX or Remote US' },
  { key: 'workAuthorization', label: 'Any work authorization constraints? (optional)', type: 'text', placeholder: 'e.g. H1B, GC, US Citizen — or leave blank' },
] as const

export default function OnboardingFlow({ onComplete, onSkip }: Props) {
  const [step, setStep] = useState(0)
  const [data, setData] = useState<CandidateProfile>({})

  const current = steps[step]
  const isLast = step === steps.length - 1

  const handleNext = (value: string) => {
    const key = current.key
    const updated = { ...data, [key]: value || undefined }
    setData(updated)
    if (isLast) onComplete(updated)
    else setStep(step + 1)
  }

  return (
    <div className="fixed inset-0 bg-black/70 flex items-center justify-center z-50 p-4">
      <div className="bg-gray-900 border border-gray-700 rounded-xl max-w-lg w-full p-6 shadow-2xl">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-lg font-semibold text-emerald-400">Welcome to DirectPath</h2>
          <button onClick={onSkip} className="text-sm text-gray-500 hover:text-gray-300">Skip</button>
        </div>
        <div className="mb-2 text-xs text-gray-500">Step {step + 1} of {steps.length}</div>
        <label className="block text-gray-200 mb-3">{current.label}</label>
        {current.type === 'select' ? (
          <div className="space-y-2">
            {current.options.map((opt) => (
              <button
                key={opt}
                onClick={() => handleNext(opt)}
                className="w-full text-left px-4 py-3 rounded-lg border border-gray-700 hover:border-emerald-500 hover:bg-gray-800 transition-colors"
              >
                {opt}
              </button>
            ))}
          </div>
        ) : (
          <form onSubmit={(e) => { e.preventDefault(); handleNext((e.target as HTMLFormElement).value.value) }}>
            <input
              name="value"
              type="text"
              placeholder={current.placeholder}
              className="w-full px-4 py-3 rounded-lg bg-gray-800 border border-gray-700 focus:border-emerald-500 focus:outline-none mb-4"
            />
            <button type="submit" className="w-full py-3 bg-emerald-600 hover:bg-emerald-500 rounded-lg font-medium transition-colors">
              {isLast ? 'Get Started' : 'Continue'}
            </button>
          </form>
        )}
      </div>
    </div>
  )
}
