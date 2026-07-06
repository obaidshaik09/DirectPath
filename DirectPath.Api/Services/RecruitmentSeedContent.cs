namespace DirectPath.Api.Services;

public static class RecruitmentSeedContent
{
  private static readonly string[] Paragraphs = GenerateParagraphs();

  public static string GetChunk(int index) =>
    index < Paragraphs.Length ? Paragraphs[index] : "";

  public static int Count => Paragraphs.Length;

  private static string[] GenerateParagraphs()
  {
    var topics = new (string Title, string[] Points)[]
    {
      ("US IT Job Market", new[] {
        "The US IT job market in 2025-2026 remains bifurcated: senior engineers in AI, cloud, and cybersecurity see strong demand while mid-level generalist roles face more competition. Companies increasingly hire for specific stack experience rather than broad 'software engineer' titles.",
        "Tech hiring cycles typically slow in Q4 due to budget freezes and accelerate in Q1-Q2 when new fiscal year budgets unlock. Candidates who time their search around these cycles often see 30-40% more interview opportunities.",
        "Major tech hubs — San Francisco, Seattle, NYC, Austin, Denver — still command premium salaries but remote roles from these companies often pay 85-95% of local rates. Secondary markets like Raleigh, Nashville, and Salt Lake City are growing rapidly.",
        "Enterprise companies (Fortune 500) are hiring more steadily than pure tech startups in the current cycle. Healthcare IT, fintech, and defense contractors offer stability with competitive compensation packages.",
        "The 'hidden job market' — roles filled through referrals and direct outreach before being posted — represents an estimated 60-70% of senior positions. DirectPath helps candidates access this channel without recruiters."
      }),
      ("Contract vs Full Time", new[] {
        "Full-time W2 employment means you are a direct employee with benefits (health insurance, 401k, PTO), payroll taxes split with employer, and at-will employment. Base salary is quoted annually before taxes.",
        "Contract W2 means you work through a staffing agency who is your legal employer. You receive hourly pay, often without benefits, but sometimes with limited PTO. The agency bills the client at a markup of 30-60% above your rate.",
        "Contract C2C (Corp-to-Corp) means you incorporate (LLC or S-Corp) and invoice the client or prime vendor directly. You handle your own taxes, benefits, and business expenses. Rates are typically 20-40% higher than W2 contract to compensate.",
        "1099 independent contractor status means you are self-employed with maximum flexibility but no employment protections. Misclassification is a serious legal issue — the IRS uses behavioral, financial, and relationship tests to determine true status.",
        "Contract roles average 3-12 month durations with extension possibilities. Full-time roles offer career growth, equity (at startups), and stability. Many senior engineers alternate between contract (higher cash) and full-time (equity + benefits) throughout their careers."
      }),
      ("C2C W2 1099", new[] {
        "C2C arrangements require you to have an active business entity (LLC is most common). You'll need an EIN, business bank account, and often general liability insurance ($1-2M coverage is standard in enterprise contracts).",
        "W2 contract through a staffing firm: the agency handles tax withholding, issues W2 at year end, and may offer limited benefits. Your hourly rate is lower than C2C because the agency takes a margin and covers employer taxes.",
        "1099 contractors invoice for services and receive a 1099-NEC form. You pay self-employment tax (15.3% for Social Security + Medicare) on net earnings. Quarterly estimated tax payments are required to avoid IRS penalties.",
        "Rate comparison example for a Senior Java Developer: W2 contract $65-75/hr, C2C $80-95/hr, 1099 $85-100/hr, Full-time equivalent $140-170K salary. The C2C premium covers taxes, insurance, and unpaid time between contracts.",
        "Some clients only allow W2 through their approved vendor list. Others prefer C2C to avoid co-employment liability. Always clarify employment type before investing hours in interviews."
      }),
      ("Bench Sales", new[] {
        "Bench sales is a staffing industry practice where recruiting firms maintain a 'bench' of consultants between contracts. When a consultant's project ends, the firm tries to place them on a new client engagement quickly.",
        "Bench sales recruiters cold-call and email IT consultants daily offering to market their resume to clients. They typically take 20-40% of the billing rate as their margin, significantly reducing your take-home pay.",
        "Red flags in bench sales: requiring you to sign exclusive representation agreements, asking for copies of your passport or SSN before any interview, promising guaranteed placements, or pressuring you to fake experience on resumes.",
        "Legitimate staffing relationships involve transparent rate discussions, no upfront fees, clear contract terms, and the recruiter explaining exactly which clients they'll submit you to. You should always know your bill rate and pay rate.",
        "DirectPath empowers candidates to bypass bench sales entirely by searching jobs directly, optimizing resumes for ATS, and writing their own outreach messages to hiring managers."
      }),
      ("ATS Systems", new[] {
        "Applicant Tracking Systems (ATS) like Workday, Greenhouse, Lever, Taleo, and iCIMS parse resumes before human recruiters see them. Up to 75% of resumes are filtered out automatically for failing keyword matching.",
        "ATS-friendly resume formatting: use standard section headers (Experience, Education, Skills), avoid tables/columns/graphics, use .docx or simple PDF, include exact keywords from the job description, and spell out acronyms at least once.",
        "Keyword optimization means mirroring the job description's language: if they say 'Kubernetes' don't only write 'K8s'. If they list 'CI/CD' include specific tools like Jenkins, GitHub Actions, or Azure DevOps.",
        "Workday ATS is used by 45%+ of Fortune 500 companies. It heavily weights exact title matches and years of experience. Greenhouse and Lever (common at startups) are more flexible but still keyword-dependent.",
        "When optimizing for ATS, maintain truthfulness — never add skills you don't have. Instead, reframe existing experience using the employer's terminology and quantify achievements with metrics."
      }),
      ("Recruiter Scripts", new[] {
        "When recruiters present candidates to clients, they use a 'hot sheet' or 'tear sheet' — a one-page summary with candidate name, top skills, rate/salary, availability, and a 3-sentence pitch highlighting why this person fits the role.",
        "The bench pitch formula: '[Name] is a [years]+ year [role] with deep expertise in [2-3 key skills]. Currently [availability], [work auth status], targeting [rate/salary]. Recent highlight: [quantified achievement].'",
        "Recruiters screen candidates in 15-20 minute calls covering: rate expectations, work authorization, availability/start date, reason for leaving, commute/remote preference, and technical stack depth.",
        "Client-facing recruiters earn 15-25% commission on placements. This incentivizes them to push candidates who are easiest to place, not necessarily the best fit. Understanding this dynamic helps you negotiate better.",
        "Top recruiters add value through market intelligence, salary benchmarking, and relationship access to hiring managers. Poor recruiters spam your resume to every opening without customization."
      }),
      ("Salary Negotiation", new[] {
        "Never share your current salary with recruiters or employers — it's illegal for employers to ask in many states (CA, NY, CO, WA, and others). Instead, ask for the role's budget range first.",
        "Research salary ranges using Levels.fyi, Glassdoor, Blind, and Bureau of Labor Statistics data. For contracts, multiply desired annual salary by 1.3-1.5 to get hourly rate (accounts for benefits gap and bench time).",
        "Negotiation timing: negotiate after receiving a verbal offer, not during initial screening. Express enthusiasm first, then present your researched range with justification based on skills and market data.",
        "Beyond base salary, negotiate: signing bonus, equity/RSUs, PTO, remote work flexibility, professional development budget, and start date. For contracts, negotiate rate, payment terms (net-30 vs net-60), and extension clauses.",
        "The 'anchoring' technique: state your ideal number first (slightly above target). Employers often meet in the middle. For a $150K target, anchor at $165K and expect settlement around $150-155K."
      }),
      ("LinkedIn Outreach", new[] {
        "LinkedIn connection request limit is ~100 per week for free accounts. Personalized messages have 3-5x higher acceptance rates than generic 'I'd like to connect' requests.",
        "Effective connection message template: 'Hi [Name], I noticed [Company] is hiring for [Role]. With [X years] in [skill], I've [specific achievement]. Would love to connect and learn more about the team.'",
        "Follow-up sequence: Day 3 after connection — thank them and express interest. Day 7 — share a relevant article or insight. Day 14 — direct ask for 15-minute informational chat about the role.",
        "Target hiring managers and team leads, not just HR. Search '[Company] [Department] Manager' or look at who posted the job listing. Engineering managers have more influence on technical hiring than recruiters.",
        "Avoid mass-messaging the same template. LinkedIn's algorithm penalizes copy-paste outreach. Customize each message with one specific detail about the company or person's recent post."
      }),
      ("Visa Work Authorization", new[] {
        "H-1B visa: employer-sponsored, 3-year initial period renewable for 3 more years, capped at 6 years (extensions possible with pending GC). Annual lottery in March with start date October 1.",
        "Green Card (Employment-Based): EB-1 (extraordinary ability), EB-2 (advanced degree + PERM labor certification), EB-3 (skilled workers). PERM process takes 12-24 months. Priority dates vary by country (India/China have long backlogs).",
        "OPT (Optional Practical Training): 12 months for all graduates, STEM extension adds 24 months (36 total). Must work in field related to degree. Unemployment limit of 90 days during OPT period.",
        "CPT (Curricular Practical Training): available during studies, no cap but must maintain full-time enrollment. Common for internships and co-op programs.",
        "EAD (Employment Authorization Document): allows work for Green Card applicants during adjustment of status. Also issued to H-4 spouses of H-1B holders with approved I-140.",
        "When job searching on visa: be upfront about sponsorship needs early. Companies with established immigration programs (most large tech) are safer bets than startups without immigration counsel."
      }),
      ("Interview Preparation", new[] {
        "Technical interviews for senior roles typically include: system design (45-60 min), coding (2-3 problems), and behavioral (1-2 rounds). Mid-level focuses more on coding and fundamentals.",
        "STAR method for behavioral questions: Situation (set the scene), Task (your responsibility), Action (specific steps you took), Result (quantified outcome). Practice 8-10 stories covering leadership, conflict, failure, and success.",
        "Common behavioral questions: 'Tell me about a time you disagreed with your manager', 'Describe a project you're most proud of', 'How do you handle tight deadlines', 'Tell me about a failure and what you learned'.",
        "System design preparation: practice designing URL shorteners, chat systems, news feeds, and rate limiters. Focus on trade-offs, scalability, and component selection rather than perfect solutions.",
        "Always prepare 3-5 thoughtful questions for the interviewer: team structure, tech stack roadmap, on-call expectations, success metrics for the role, and engineering culture values."
      }),
      ("Job Portals", new[] {
        "LinkedIn Jobs: largest professional network, best for direct networking and recruiter inbound. Premium Career ($30/mo) shows applicant insights and InMail credits.",
        "Indeed: highest job volume, strong for full-time roles across all industries. Easy Apply feature means high competition — customize each application when possible.",
        "Dice: IT-specific board, strongest for contract roles. Recruiters actively search candidate profiles. Keep Dice resume updated with contract availability and rate.",
        "FlexJobs: curated remote and flexible positions, subscription required ($15/mo). Quality over quantity — fewer but more legitimate remote listings.",
        "Built In: startup and tech company focused, strong in specific cities (Built In Austin, NYC, etc.). Good for culture-fit research alongside job listings.",
        "Google Jobs: aggregates from multiple sources. Good for discovery but apply on the original source for better tracking.",
        "Hired, Triplebyte (now part of Karat), and Toptal: assessment-based platforms for pre-vetted candidates. Higher bar to entry but less competition per role."
      }),
      ("Recruiter Red Flags", new[] {
        "Red flag: recruiter won't disclose client company name before you submit resume. Legitimate recruiters share client info after a brief screening call.",
        "Red flag: asked to modify resume to include skills you don't have ('just add AWS, they won't check'). This is resume fraud and grounds for termination.",
        "Red flag: upfront fees for job placement. Legitimate staffing firms are paid by employers, never candidates.",
        "Red flag: pressure to sign exclusive contracts preventing you from working with other recruiters or applying directly to companies.",
        "Red flag: vague job descriptions with unrealistically high rates ('$120/hr for entry-level Java'). If it sounds too good to be true, verify the client exists.",
        "Red flag: recruiter doesn't know basic details about the role — tech stack, team size, project duration. They're blasting resumes without vetting."
      }),
      ("Rate Negotiation Contractors", new[] {
        "Calculate your minimum bill rate: (Desired Annual Salary / 2080 hours) × 1.35 overhead factor. For $150K equivalent: ($150K/2080) × 1.35 = ~$97/hr C2C minimum.",
        "Negotiate payment terms: Net-15 is ideal, Net-30 is standard, Net-45+ is a cash flow risk. Include late payment penalties in your contract.",
        "Bench time between contracts averages 2-6 weeks annually. Factor this into rate calculations — you need 10-15% higher contract rate vs equivalent salary to break even.",
        "Rate negotiation script: 'Based on my [X years] experience with [skills] and current market rates of [$range], I'm targeting [$your rate]. I'm flexible on start date and project scope.'",
        "When a client counters low, offer scope adjustments: 'I can meet that rate for a 6-month commitment' or 'That rate works if remote with no on-site requirement.'"
      }),
      ("Tech Hiring Trends", new[] {
        "AI/ML engineering roles grew 40%+ in 2024-2025 but require demonstrated production ML experience, not just coursework. LLM fine-tuning, RAG pipelines, and MLOps are the most requested sub-skills.",
        "Cloud platform expertise (AWS, Azure, GCP) remains the most transferable skill. Multi-cloud experience commands 10-15% rate premium over single-platform specialists.",
        "Cybersecurity roles face a 3.5 million global worker shortage. Security engineers, cloud security architects, and GRC analysts are among the fastest-growing IT positions.",
        "Full-stack JavaScript (React/Node) remains the most common job posting stack, but competition is highest. Specialized niches (Rust, Go systems programming, embedded) have fewer candidates per opening.",
        "Return-to-office mandates have created a split market: RTO-required roles in major cities see 20-30% fewer applicants, creating negotiation leverage for candidates willing to be on-site.",
        "Tech layoffs in 2024-2025 primarily affected non-engineering roles (recruiting, PM, marketing) and over-hired junior cohorts. Senior engineers (5+ years) with production experience remain in demand."
      })
    };

    var paragraphs = new List<string>();
    for (var round = 0; round < 8; round++)
    {
      foreach (var (title, points) in topics)
      {
        foreach (var point in points)
        {
          paragraphs.Add($"[{title} — Insight {round + 1}] {point} This knowledge helps IT candidates navigate the US job market independently without relying on recruiting agencies or bench sales intermediaries.");
        }
      }
    }
    return paragraphs.ToArray();
  }
}
