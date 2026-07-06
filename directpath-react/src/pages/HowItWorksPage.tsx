import { Link } from 'react-router-dom'
import { UserCircle, Search, FileText, MessageSquare, Trophy, Database } from 'lucide-react'

const steps = [
  { icon: UserCircle, title: 'Tell us about yourself', desc: 'Complete the quick onboarding to personalize your experience' },
  { icon: Search, title: 'Search for jobs', desc: 'We find real full time and contract roles matching your skills and preferences' },
  { icon: FileText, title: 'Optimize your resume', desc: 'AI rewrites your resume to pass ATS filters for each specific job you apply to' },
  { icon: MessageSquare, title: 'Reach out directly', desc: 'Get personalized LinkedIn messages to contact hiring managers yourself, no agency needed' },
  { icon: Trophy, title: 'Prepare and land the job', desc: 'Practice interviews, know your market rate, skip the middleman entirely' },
]

export default function HowItWorksPage() {
  return (
    <div className="max-w-4xl mx-auto px-4 py-12">
      <div className="text-center mb-12">
        <h1 className="text-4xl font-bold mb-3">How DirectPath Works</h1>
        <p className="text-lg text-gray-400">Everything a recruiter does for you — done by AI, for free</p>
      </div>

      <div className="space-y-8 mb-16">
        {steps.map((step, i) => (
          <div key={i} className="flex gap-6 items-start">
            <div className="flex-shrink-0 w-12 h-12 rounded-full bg-emerald-500/10 border border-emerald-500/30 flex items-center justify-center">
              <step.icon className="w-6 h-6 text-emerald-400" />
            </div>
            <div>
              <div className="text-sm text-emerald-400 font-medium mb-1">Step {i + 1}</div>
              <h3 className="text-xl font-semibold mb-2">{step.title}</h3>
              <p className="text-gray-400">{step.desc}</p>
            </div>
          </div>
        ))}
      </div>

      <div className="bg-gray-900 border border-gray-800 rounded-xl p-8 mb-12">
        <div className="flex items-center gap-3 mb-4">
          <Database className="w-6 h-6 text-emerald-400" />
          <h2 className="text-2xl font-semibold">Powered by Recruiter Knowledge</h2>
        </div>
        <p className="text-gray-400 leading-relaxed">
          DirectPath searches a knowledge base of 500+ real recruitment insights before answering your questions —
          so you get insider recruiter knowledge, not just generic AI responses. Every answer is backed by real data
          with a minimum 50% relevance threshold.
        </p>
      </div>

      <div className="text-center">
        <Link
          to="/"
          className="inline-block px-8 py-3 bg-emerald-600 hover:bg-emerald-500 rounded-lg font-medium transition-colors"
        >
          Start Now
        </Link>
      </div>
    </div>
  )
}
