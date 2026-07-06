import { useState, useEffect } from 'react'
import type { CandidateProfile } from '../api/client'

export function useProfile() {
  const [profile, setProfile] = useState<CandidateProfile | null>(null)

  useEffect(() => {
    const raw = localStorage.getItem('directpath_profile')
    if (raw) setProfile(JSON.parse(raw))
  }, [])

  const saveProfile = (p: CandidateProfile) => {
    localStorage.setItem('directpath_profile', JSON.stringify(p))
    localStorage.setItem('directpath_onboarded', 'true')
    setProfile(p)
  }

  const isOnboarded = () => localStorage.getItem('directpath_onboarded') === 'true'

  return { profile, saveProfile, isOnboarded }
}
