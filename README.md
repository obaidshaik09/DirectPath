# DirectPath — Be Your Own Recruiter

> Skip the middleman. Find your job, your way.

DirectPath is an AI-powered personal recruitment assistant that gives candidates everything a recruiter or bench sales agency does — completely free, through AI. Whether you are looking for a full-time role or a contract position (C2C/W2/1099), DirectPath guides you from zero to hired without needing anyone in between.

## Why DirectPath?

The US IT recruitment industry is full of middlemen — staffing agencies, bench sales operations, and recruiters who take 15–25% of your salary just to connect you with a job you could have found yourself. DirectPath changes that. It puts recruiter-level knowledge directly in your hands so you can search smarter, apply better, and negotiate confidently.

## What It Does

- **Profile Builder** — generates a professional recruiter-style candidate pitch from your background
- **Resume Optimizer** — rewrites your resume to pass ATS filters for any specific job description
- **Job Search** — finds real full-time and contract roles matching your skills and location
- **Salary Calculator** — tells you your real market rate before you talk to anyone
- **Outreach Generator** — creates personalized LinkedIn messages to reach hiring managers directly
- **Interview Prep** — gives you tailored technical and behavioral questions with STAR method coaching
- **Recruitment News** — live feed of US IT hiring trends, layoffs, in-demand skills, and visa updates
- **Ask Anything** — general AI chat backed by a 500+ chunk recruiter knowledge base

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | React, Tailwind CSS |
| Backend | .NET 10 Web API |
| Database | PostgreSQL 17 + pgvector |
| Embeddings | OpenRouter (text-embedding-3-small) |
| AI | Anthropic Claude |
| RAG | Cosine similarity search, 50% threshold |
| Deployment | AWS EC2, Nginx |

## How It Works

1. User tells DirectPath their background via the onboarding flow
2. Any question or task triggers a RAG search across 500+ recruitment knowledge chunks
3. Only results with ≥50% cosine similarity are passed to Claude as context
4. Claude generates a precise, recruiter-informed answer
5. If nothing relevant is found in the knowledge base, it falls back to Claude's general knowledge

## Getting Started

### Prerequisites
- .NET 10 SDK
- Node.js 20+
- PostgreSQL 17 with pgvector extension
- OpenRouter API key
- Anthropic API key

### Setup

```bash
# Clone the repo
git clone https://github.com/obaidshaik09/DirectPath.git
cd DirectPath

# Backend
cd DirectPath.Api
cp .env.example .env
# Add your real keys to .env
dotnet run

# Frontend (new terminal)
cd directpath-react
npm install
npm run dev
```

### Environment Variables
Create DirectPath.Api/.env with:

```
OPENROUTER_API_KEY=your_openrouter_key_here
ANTHROPIC_API_KEY=your_anthropic_key_here
POSTGRES_PASSWORD=your_postgres_password_here
```

Never commit `.env` — it is gitignored. Use `.env.example` as a template with placeholders only.
