using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AxelLinaresApi.Data;
using AxelLinaresApi.Models;

namespace AxelLinaresApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileDbContext _context;

        public ProfileController(ProfileDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserProfile), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile([FromQuery] string lang = "es")
        {
            var profile = await _context.UserProfiles
                .Include(u => u.Experiences)
                .Include(u => u.Projects)
                .Include(u => u.Certifications)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (profile == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    type: "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    title: "Perfil no encontrado",
                    detail: "No se encontró el perfil profesional en el sistema.");
            }

            if (string.Equals(lang, "en", StringComparison.OrdinalIgnoreCase))
            {
                profile.Title = "Software Engineer | Backend (.NET/C#) & QA Automation";
                profile.Summary = "Software Engineer with 3 years of experience designing robust Backend services (.NET/C#) and building test automation architectures (QA). Specialized in REST API development, advanced SQL database optimization, and the creation of automated End-to-End (E2E) suites using Playwright. Focused on ensuring high availability, security, and scalability in agile ecosystems and cloud-native deployments (AWS).";

                foreach (var exp in profile.Experiences)
                {
                    if (exp.Company.Contains("Positronica"))
                    {
                        exp.JobTitle = "Software Engineer (Co-Founder)";
                        exp.CompanyUrl = "https://positronicalabs.dev";
                        exp.Dates = "January 2026 - Present";
                        exp.Responsibilities = new List<string>
                        {
                            "Technical leadership and backend/full-stack development in the founding of Positronica Labs (positronicalabs.dev), defining the baseline architecture and business logic for the initial product launch.",
                            "Implementation of Clean Architecture for the .NET Core ecosystem, managing scalable cloud infrastructure deployment via AWS and Render.",
                            "Design of E2E automation architectures (Playwright / Browserbase) and AI agent orchestration workflows (n8n.cloud, Backboard.io, Firecrawl), optimizing deployment cycles and ensuring software quality."
                        };
                    }
                    else if (exp.Company.Contains("Sección Amarilla"))
                    {
                        exp.JobTitle = "QA Automation Intern (Social Service)";
                        exp.CompanyUrl = "https://www.seccionamarilla.com.mx/";
                        exp.Dates = "March 2026 – July 2026";
                        exp.Responsibilities = new List<string>
                        {
                            "Design, architecture, and from-scratch implementation of automated E2E and black-box test suites for the core 'PINBOX' portal using C#, .NET, and Playwright.",
                            "Refactoring and stabilization of test scripts, implementing Page Object Model (POM) design patterns to minimize code fragility and facilitate scalability.",
                            "Integration of quality engineering practices (Shift-Left Testing) within agile development cycles (Scrum), ensuring early detection of critical defects and pre-production regressions.",
                            "Git repository management, establishing version control policies and .gitignore rules to safeguard credentials and maintain clean environments."
                        };
                    }
                    else if (exp.Company.Contains("Solera"))
                    {
                        exp.JobTitle = "Associate Software Engineer I";
                        exp.CompanyUrl = "https://www.solera.com";
                        exp.Dates = "August 2022 – February 2025";
                        exp.Responsibilities = new List<string>
                        {
                            "Development, maintenance, and evolution of high-concurrency enterprise systems (Backend & Full-Stack) using C#, .NET Framework, Java, and CFML.",
                            "Continuous engineering and support of client-server applications (Console and Windows Forms), ensuring business logic integrity and end-user experience.",
                            "Deep performance optimization of relational databases, refactoring stored procedures, complex queries, and analyzing SQL Server execution plans, reducing system response times by 30%.",
                            "Active collaboration with multidisciplinary teams under agile methodologies (SCRUM) to standardize deployment pipelines and streamline continuous integration flows."
                        };
                    }
                }

                foreach (var proj in profile.Projects)
                {
                    if (proj.Title.Contains("BenchEngine") || proj.Title.Contains("GreenGemma"))
                    {
                        proj.Title = "BenchEngine / GreenGemma (MLH Hackathon)";
                        proj.Badge = "🏆 4th Place MLH Hackathon";
                        proj.Description = "Awarded Honorable Mention (4th place) at MLH. Local energy benchmarking platform for open LLMs (Gemma 4) integrating structured web extraction via Firecrawl, OTP auth, and ElevenLabs voice synthesis.";
                        proj.ArchitectureNotes = new List<string>
                        {
                            "Local vs Cloud Inference: Runs Gemma 4 on Ollama to measure real energy consumption per token with zero network latency.",
                            "Structured Web Ingestion: Integrates Firecrawl to transform technical docs into clean evaluation contexts.",
                            "Accessibility & Security: Passwordless flow via MojoAuth OTP and real-time audio telemetry using ElevenLabs API."
                        };
                    }
                    else if (proj.Title.Contains("Voz Abierta"))
                    {
                        proj.Title = "Voz Abierta — Bidirectional 3D Sign Language Translator";
                        proj.Badge = "🚀 AI Hackathon × Clara";
                        proj.Description = "Prototype built for AI Hackathon por México × Clara. Real-time bidirectional translator between Mexican Sign Language (LSM) and Spanish using MediaPipe computer vision, a Three.js 3D avatar, and Web Speech API.";
                        proj.ArchitectureNotes = new List<string>
                        {
                            "Edge Computer Vision: 60 FPS hand and body landmark extraction directly in the browser via MediaPipe without streaming raw video externally.",
                            "3D Animation Engine: Kinematic interpolation in Three.js translating speech/text into fluid sign animations.",
                            "Civic Impact: Low-latency architecture engineered for accessibility in public service desks and financial institutions."
                        };
                    }
                    else if (proj.Title.Contains("Voice2Task"))
                    {
                        proj.Title = "Voice2Task — Native iOS Voice-to-GitHub Agent";
                        proj.Badge = "📱 Native iOS + AI Agents";
                        proj.Description = "Native iOS app (SwiftUI + SwiftData) that transcribes technical voice notes in real time with SFSpeechRecognizer, extracts structured tasks via OpenAI (gpt-4o-mini), and syncs them directly with GitHub Issues.";
                        proj.ArchitectureNotes = new List<string>
                        {
                            "Local-First Persistence: Reactive storage with SwiftData guaranteeing zero data loss even offline.",
                            "Deterministic LLM Extraction: Structured JSON Schema prompts on gpt-4o-mini transforming free-form audio into actionable tickets with labels and acceptance criteria.",
                            "Bidirectional Sync: Direct integration with GitHub Issues REST API featuring resilient retry and rate-limit handling."
                        };
                    }
                    else if (proj.Title.Contains("Panaderías"))
                    {
                        proj.Title = "Enterprise Bakery ERP & POS System";
                        proj.Badge = "🏛️ Clean Architecture & ACID";
                        proj.Description = "Full-Stack enterprise ecosystem built under Clean Architecture with .NET 8 Web API, Entity Framework Core, and SQL Server. Implements multi-ingredient inventory control with ACID transactions and a React 18 SPA.";
                        proj.ArchitectureNotes = new List<string>
                        {
                            "ACID Transactional Consistency: Explicit EF Core transactions deducting recipe ingredients and recording sales without race conditions.",
                            "Layered Decoupling: Strict separation across Domain, Application, Infrastructure, and Presentation layers enabling isolated unit testing.",
                            "SQL Optimization: Composite indexes in SQL Server reducing inventory and reporting query latency by 30%."
                        };
                    }
                    else if (proj.Title.Contains("Portafolio") || proj.Title.Contains("Cloud Engine"))
                    {
                        proj.Title = "Cloud Portfolio Engine & API Core (.NET 8 + AWS)";
                        proj.Badge = "☁️ AWS S3 + CloudFront + .NET 8";
                        proj.Description = "Decoupled Full-Stack platform deployed on AWS (S3, CloudFront, and Elastic Beanstalk). .NET 8 backend featuring native IP Rate Limiting, AppSec CRLF sanitization, and standardized RFC 7807 ProblemDetails.";
                        proj.ArchitectureNotes = new List<string>
                        {
                            "Anti-Spam Resilience (Rate Limiting): FixedWindowRateLimiter partitioned by client IP (X-Forwarded-For) returning HTTP 429 in RFC 7807 format.",
                            "Low-Latency Queries: Read-only queries optimized with EF Core AsNoTracking() and in-memory bilingual negotiation (?lang=es|en).",
                            "AppSec Hardening: Strict SMTP header sanitization and DataAnnotations validation mitigating OWASP vulnerabilities."
                        };
                    }
                    else if (proj.Title.Contains("TaskMaster"))
                    {
                        proj.Title = "TaskMaster API — Modern CFML Showcase";
                        proj.Badge = "🛡️ AppSec & OWASP Top 10";
                        proj.Description = "Production-grade RESTful API built in 100% modern cfscript on Lucee and CommandBox. Features layered architecture, stateless HMAC-SHA256 JWT authentication, embedded H2 database, and RFC 7807 error contracts.";
                        proj.ArchitectureNotes = new List<string>
                        {
                            "Legacy Ecosystem Modernization: Demonstrates enterprise-grade CFML engineering using 100% pure cfscript decoupled from views.",
                            "Stateless Security: HMAC-SHA256 signed JWT tokens and 100% parameterized queries preventing SQL Injection.",
                            "Standardized Error Contracts: Global exception mapping into RFC 7807 ProblemDetails payloads."
                        };
                    }
                    else if (proj.Title.Contains("QA AI-Assistant"))
                    {
                        proj.Title = "QA AI-Assistant & Enterprise Playwright Suite";
                        proj.Badge = "🤖 Playwright + OpenAI API";
                        proj.Description = "End-to-End (E2E) automation framework in C# / .NET 9, Playwright, and NUnit integrated with a Python / OpenAI API analytical assistant that captures runtime exceptions to diagnose root causes and suggest patches in real time.";
                        proj.ArchitectureNotes = new List<string>
                        {
                            "AI-Assisted Diagnostics: Automatic capture of stack traces, DOM snapshots, and Playwright logs sent to OpenAI models for real-time fix suggestions.",
                            "Zero-Flakiness Architecture: Implements Page Object Model (POM), async LocatorHandlers, and synthetic data factories with Bogus.",
                            "E2E Observability: Automated generation of network HAR files and execution traces compatible with Browserbase cloud sessions."
                        };
                    }
                    else if (proj.Title.Contains("Chamba"))
                    {
                        proj.Title = "Platform 'Mi Primera Chamba' (QA Tester Team)";
                        proj.Badge = "🎯 Shift-Left QA Team";
                        proj.Description = "Member of the Quality Assurance (QA) team executing functional testing, critical user flow validation, and early defect reporting (Shift-Left Testing) to guarantee platform stability prior to production rollouts.";
                        proj.ArchitectureNotes = new List<string>
                        {
                            "Team Quality Assurance (Shift-Left): Direct collaboration with the engineering team validating acceptance criteria prior to production merges.",
                            "Critical Flow Validation: Design and execution of test matrices covering onboarding, authentication, candidate profiles, and job matching.",
                            "Defect Traceability: Structured bug documentation with reproducible steps, network/console evidence, and zero-regression hotfix verification."
                        };
                    }
                    else if (proj.Title.Contains("Algoritmia") || proj.Title.Contains("OMI"))
                    {
                        proj.Title = "Advanced Algorithms & Competitive Programming (OMI)";
                        proj.Badge = "⚡ C++ Advanced DSA";
                        proj.Description = "Repository of 50+ high-performance C++ solutions for the Mexican Olympiad in Informatics (OMI) and online judges, implementing advanced data structures and time/space complexity optimization.";
                        proj.ArchitectureNotes = new List<string>
                        {
                            "Advanced Data Structures: From-scratch implementations of Persistent Segment Trees, Implicit Treaps, Fenwick Trees, and Disjoint Set Union (DSU).",
                            "Extreme CPU & Memory Optimization: Solutions engineered to pass strict execution bounds (< 1 second) and tight memory limits.",
                            "Algorithmic Foundation: Applied directly to efficient SQL query design and backend engine optimization."
                        };
                    }
                }

                profile.SoftSkills = new List<string> 
                { 
                    "Technical Leadership",
                    "Autonomous Learning (Self-taught)", 
                    "Complex Problem Solving",
                    "Effective Technical Communication",
                    "Technological Adaptability",
                    "Critical & Analytical Thinking",
                    "Shift-Left Testing & Quality Culture",
                    "Resilience Under Pressure"
                };
            }

            return Ok(profile);
        }
    }
}
