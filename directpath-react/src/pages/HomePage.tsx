import { Link } from 'react-router-dom'
import { User, FileText, Search, DollarSign, MessageSquare, Mic, Newspaper, MessageCircle } from 'lucide-react'

const features = [
  { to: '/profile', icon: User, title: 'Build my profile', desc: 'Create a recruiter-style candidate profile and bench pitch' },
  { to: '/resume', icon: FileText, title: 'Optimize my resume', desc: 'ATS-optimize your resume for any job description' },
  { to: '/jobs', icon: Search, title: 'Search jobs', desc: 'Find real full-time and contract IT roles' },
  { to: '/salary', icon: DollarSign, title: 'Know my market rate', desc: 'Get salary and contract rate ranges for your role' },
  { to: '/outreach', icon: MessageSquare, title: 'Write outreach messages', desc: 'Personalized LinkedIn messages to hiring managers' },
  { to: '/interview', icon: Mic, title: 'Prepare for interviews', desc: 'Technical and behavioral questions with STAR coaching' },
  { to: '/news', icon: Newspaper, title: 'Recruitment news', desc: 'Latest US IT job market news and trends' },
  { to: '/chat', icon: MessageCircle, title: 'Ask anything', desc: 'Chat with AI recruiter knowledge — contracts, negotiation, red flags' },
]

export default function HomePage() {
  return (
    <div className="max-w-6xl mx-auto px-4 py-12">
      <div className="text-center mb-16">
        <h1 className="text-5xl font-bold mb-4 bg-gradient-to-r from-emerald-400 to-teal-300 bg-clip-text text-transparent">
          Be your own recruiter
        </h1>
        <p className="text-xl text-gray-400 max-w-2xl mx-auto">
          DirectPath is your AI-powered personal recruiter. Find jobs, optimize resumes, and land roles — no agency needed.
        </p>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        {features.map((f) => (
          <Link
            key={f.to}
            to={f.to}
            className="group p-6 rounded-xl border border-gray-800 bg-gray-900/50 hover:border-emerald-500/50 hover:bg-gray-900 transition-all"
          >
            <f.icon className="w-8 h-8 text-emerald-400 mb-4 group-hover:scale-110 transition-transform" />
            <h3 className="font-semibold text-gray-100 mb-2">{f.title}</h3>
            <p className="text-sm text-gray-500">{f.desc}</p>
          </Link>
        ))}
      </div>
    </div>
  )
}
