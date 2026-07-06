import { useState } from 'react'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Navbar from './components/Navbar'
import OnboardingFlow from './components/OnboardingFlow'
import HomePage from './pages/HomePage'
import HowItWorksPage from './pages/HowItWorksPage'
import ProfileBuilder from './components/ProfileBuilder'
import ResumeOptimizer from './components/ResumeOptimizer'
import JobSearch from './components/JobSearch'
import SalaryCalculator from './components/SalaryCalculator'
import OutreachGenerator from './components/OutreachGenerator'
import InterviewPrep from './components/InterviewPrep'
import NewsPortal from './components/NewsPortal'
import ChatInterface from './components/ChatInterface'
import AdminPanel from './components/AdminPanel'
import { useProfile } from './hooks/useProfile'

function App() {
  const { saveProfile, isOnboarded } = useProfile()
  const [showOnboarding, setShowOnboarding] = useState(!isOnboarded())

  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-950">
        <Navbar />
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/how-it-works" element={<HowItWorksPage />} />
          <Route path="/profile" element={<ProfileBuilder />} />
          <Route path="/resume" element={<ResumeOptimizer />} />
          <Route path="/jobs" element={<JobSearch />} />
          <Route path="/salary" element={<SalaryCalculator />} />
          <Route path="/outreach" element={<OutreachGenerator />} />
          <Route path="/interview" element={<InterviewPrep />} />
          <Route path="/news" element={<NewsPortal />} />
          <Route path="/chat" element={<ChatInterface />} />
          <Route path="/admin" element={<AdminPanel />} />
        </Routes>
        {showOnboarding && (
          <OnboardingFlow
            onComplete={(p) => { saveProfile(p); setShowOnboarding(false) }}
            onSkip={() => { localStorage.setItem('directpath_onboarded', 'true'); setShowOnboarding(false) }}
          />
        )}
      </div>
    </BrowserRouter>
  )
}

export default App
