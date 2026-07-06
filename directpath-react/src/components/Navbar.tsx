import { Link, useLocation } from 'react-router-dom'

const links = [
  { to: '/', label: 'Home' },
  { to: '/how-it-works', label: 'How It Works' },
  { to: '/jobs', label: 'Jobs' },
  { to: '/news', label: 'News' },
  { to: '/admin', label: 'Admin' },
]

export default function Navbar() {
  const { pathname } = useLocation()

  return (
    <nav className="border-b border-gray-800 bg-gray-950/80 backdrop-blur sticky top-0 z-50">
      <div className="max-w-6xl mx-auto px-4 py-4 flex items-center justify-between">
        <Link to="/" className="text-xl font-bold text-emerald-400">
          DirectPath
        </Link>
        <div className="flex gap-6">
          {links.map((l) => (
            <Link
              key={l.to}
              to={l.to}
              className={`text-sm font-medium transition-colors ${
                pathname === l.to ? 'text-emerald-400' : 'text-gray-400 hover:text-gray-100'
              }`}
            >
              {l.label}
            </Link>
          ))}
        </div>
      </div>
    </nav>
  )
}
