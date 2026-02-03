# FastTrak API Development - Senior Architect Guidance

## Context
You're guiding a novice developer building their first production mobile app. They're learning .NET MAUI, API architecture, and deployment fundamentals through FastTrak—a nutrition tracking app for fast-food customization.

## Your Role
- **Mentor first, architect second**: Explain *why* before *what*
- **Teach patterns, not shortcuts**: Build sustainable knowledge
- **Anticipate gaps**: Database design, security, API integration, deployment
- **Real-world focus**: Production-ready, not just working code

## App Core Requirements
**FastTrak** consolidates fast-food menu nutrition data with customization support (milk swaps, sauces, portions). Users log meals, see daily macro totals, and get accurate calculations without manual effort.

**Technical Stack**: .NET MAUI (mobile), MVVM pattern, local persistence, cloud data sync

## Guidance Principles
1. **Architecture decisions**: Explain trade-offs (REST vs GraphQL, SQL vs NoSQL)
2. **Security by default**: Auth, data validation, API keys from day one
3. **Mobile-API integration**: How MAUI consumes APIs, caching, offline-first
4. **Scalability considerations**: Start simple, plan for growth
5. **Deployment roadmap**: Local → staging → production pipeline

## Communication Style
- Concise explanations with "Why this matters" context
- Code examples only when clarifying concepts
- Flag common pitfalls preemptively
- Encourage questions; assume zero prior API knowledge
- **After each implementation step**: Provide a concept breakdown explaining the IT/dev concepts used and what to learn more about

## Success Metrics
Developer understands enough to:
- Design normalized nutrition database schema
- Build secure REST endpoints for menu CRUD operations
- Implement mobile data sync with conflict resolution
- Deploy to cloud with basic monitoring

Prioritize teaching sustainable practices over rapid feature completion.

---

# Project Baseline (January 2026)

## Current Architecture

### Data Layer
- **Database**: SQLite (local, via sqlite-net-pcl)
- **Repository**: `NutritionRepository.cs` - single class handling all data access
- **Pattern**: Direct SQLite queries, no interface abstraction

### Models (6 entities)
| Model | Purpose | Migration Target |
|-------|---------|------------------|
| Restaurant | Restaurant metadata | API |
| MenuItem | Food items with nutrition | API |
| CustomOption | Customization options (toppings, milk types) | API |
| MenuItemOption | Junction: MenuItem ↔ CustomOption | API |
| LoggedItem | User's daily food log | SQLite (private) |
| LoggedItemOption | Selected customizations per log entry | SQLite (private) |

### ViewModels (6 total)
- `HomeViewModel` - Dashboard with daily totals
- `RestaurantsViewModel` - Restaurant list (needs API service)
- `MenuItemsViewModel` - Menu items for restaurant (needs API service)
- `CustomizationViewModel` - Item customization (needs API service)
- `CalculatorViewModel` - Daily log management (SQLite only)
- `FatSecretSearchViewModel` - External food search (already uses HTTP)

### Services
- `FatSecretService.cs` - External API integration (OAuth2, working)

### Seed Data
- 4 restaurants (Wendy's, Dunkin', Wingstop, Culver's)
- ~100+ menu items with full nutrition data
- ~40+ customization options
- Junction table links

---

## Migration Plan

### Phase 1: Code Cleanup ✅ COMPLETE
1. ~~Extract `INutritionRepository` interface~~ → Created `IUserLogRepository`
2. ~~Create `IRestaurantDataService` interface for API-bound operations~~ → Done
3. ~~Separate concerns: user data vs reference data~~ → ViewModels updated

### Phase 2: API Development (CURRENT)

**Architecture Decisions (Jan 2026):**
- **API Style**: REST (caching-friendly, MAUI-compatible)
- **Hosting**: Azure App Service + Azure SQL (free tiers)
- **Auth**: Public read with rate limiting (100 req/min/IP)
- **Versioning**: URL-based (`/api/v1/`)

**Phase 2A: Foundation ✅ COMPLETE**
1. [x] Create `FastTrak.Api` project (ASP.NET Core 9 Minimal APIs)
2. [x] Add packages: EF Core, Swagger, Serilog, AspNetCoreRateLimit
3. [x] Create `FastTrakDbContext` with four entities
4. [x] Implement endpoints mapping to `IRestaurantDataService`:
   - `GET /api/v1/restaurants`
   - `GET /api/v1/restaurants/{id}/menu-items`
   - `GET /api/v1/menu-items/{id}`
   - `GET /api/v1/menu-items/{id}/options`
5. [x] Migrate seed data to SQL Server LocalDB (61 menu items, 36 options)
6. [x] Add Swagger documentation
7. [x] Standardize nutrition types to `decimal`
8. [x] Enable rate limiting middleware (100 req/min/IP)
9. [x] Add `/health` endpoint

**Phase 2A-fix: API Hardening ✅ COMPLETE (Feb 2026)**
1. [x] Fix MAUI MenuItem `double` → `decimal` type mismatch
2. [x] Add CORS policy (AllowAll for development)
3. [x] Add centralized exception-handling middleware
4. [x] Configure Serilog logging (console output)
5. [x] Add security headers (X-Content-Type-Options, X-Frame-Options)
6. [x] Add CreatedAt/UpdatedAt timestamps to API models
7. [x] Add database indexes (MenuItem.Category, CustomOption.Category, MenuItemOption.CustomOptionId)
8. [x] Create EF migration `AddTimestampsAndIndexes`

**Phase 2B: Azure Deployment**
1. [ ] Create Azure resource group (`fasttrak-rg`)
2. [ ] Provision Azure SQL Database (free tier)
3. [ ] Provision Azure App Service (F1 free tier)
4. [ ] Set up GitHub Actions CI/CD
5. [ ] Deploy and verify endpoints

**Phase 2C: Security & Performance Polish**
1. [x] Enable rate limiting middleware
2. [x] Add security headers middleware
3. [ ] Add HTTP caching headers (Cache-Control, ETag)
4. [ ] Configure Application Insights
5. [x] Add `/health` endpoint
6. [ ] Add in-memory caching for reference data

### Phase 3: MAUI Integration ✅ MOSTLY COMPLETE
1. [x] Create `RestaurantApiService` implementing `IRestaurantDataService`
2. [x] Update `MauiProgram.cs` DI registration (HttpClient factory)
3. [ ] Add retry logic with Polly (optional)
4. [ ] Implement offline cache fallback (optional)
5. [ ] End-to-end testing with local API

### Phase 4: Deployment
1. Deploy API to Azure/cloud
2. Configure MAUI app for production API
3. Remove local seed data

---

## Repository Methods

### Moving to API (obsolete after migration)
- `GetRestaurantsAsync()`
- `GetMenuItemsForRestaurantAsync(int restaurantId)`
- `GetMenuItemAsync(int id)`
- `GetCustomOptionsForMenuItemAsync(int menuItemId)`
- All `Seed*Async()` methods

### Staying in SQLite (user data)
- `GetLoggedItemsForTodayAsync()`
- `InsertLoggedItemAsync()`
- `UpdateLoggedItemAsync()`
- `DeleteLoggedItemAsync()`
- `InsertLoggedItemOptionAsync()`
- `ClearLoggedItemsForTodayAsync()`

---

## Key Files

```
FastTrak/                            # MAUI App
├── Data/
│   ├── NutritionRepository.cs      # User log data (SQLite)
│   └── Seeds/                       # Remove after API migration
├── Models/                          # Keep as-is (shared DTOs)
├── ViewModels/
├── Services/
│   ├── FatSecretService.cs         # External API (working)
│   └── IRestaurantDataService.cs   # Interface for API service
├── MauiProgram.cs
└── CLAUDE.md

FastTrak.Api/                        # REST API (NEW)
├── Program.cs                       # Entry point, middleware config
├── Data/
│   ├── FastTrakDbContext.cs        # EF Core context
│   └── DbSeeder.cs                 # Seeds DB on startup
├── Models/                          # API versions of entities
├── Endpoints/
│   ├── RestaurantEndpoints.cs
│   └── MenuItemEndpoints.cs
└── appsettings.json                 # Connection string, rate limits
```

---

## Commands

```bash
# MAUI App (build only the MAUI project, not the solution)
dotnet build FastTrak.csproj -f net8.0-windows10.0.19041.0
dotnet build FastTrak.csproj -f net8.0-android

# API (must be running for MAUI app to load restaurant data)
cd FastTrak.Api
dotnet build
dotnet run --urls "http://*:5050"    # Listen on all interfaces, port 5050
# Swagger UI: http://localhost:5050/swagger
# Health check: http://localhost:5050/health

# EF Core Migrations (run from FastTrak.Api directory)
dotnet ef migrations add <Name>
dotnet ef database update

# Kill stuck API process (if build fails with "file locked")
taskkill //F //IM FastTrak.Api.exe
```

**Note:** The MAUI app connects to `http://127.0.0.1:5050`. Start the API first before running the app.

---

## Branch Strategy

Create a new branch for each work session or feature to keep history clean:

```bash
# Feature work
git checkout -b feature/api-migration
git checkout -b feature/offline-sync

# Bug fixes
git checkout -b fix/calculation-error

# Cleanup/refactoring
git checkout -b cleanup/code-quality
```

**Why this helps:**
- Each branch = one logical unit of work
- Easy to review changes in isolation
- Can abandon failed experiments without affecting main
- Clear git history for future reference

**Branch naming:**
- `feature/*` - new functionality
- `fix/*` - bug fixes
- `cleanup/*` - refactoring, code quality
- `docs/*` - documentation only

---

## Session Log

| Date | Branch | Work Done |
|------|--------|-----------|
| Jan 2026 | `cleanup/code-quality` | Phase 1 complete - extracted interfaces, decoupled ViewModels |
| Jan 2026 | `feature/api-migration` | Phase 2 planning - architecture decisions, endpoint design |
| Jan 21 2026 | `feature/api-migration` | Phase 2A complete - API project created, endpoints, DB seeded, rate limiting |
| Feb 3 2026 | `feature/api-migration` | Phase 2A-fix complete - CORS, error handling, Serilog, timestamps, indexes. API assessment completed. Data acquisition strategy documented. |

---

## API Project Structure (Created)

```
FastTrak.Api/
├── Program.cs                    # Entry point, DI, rate limiting, endpoints
├── appsettings.json              # Connection string, rate limit config
├── appsettings.Development.json
├── Data/
│   ├── FastTrakDbContext.cs     # EF Core context with 4 DbSets
│   ├── DbSeeder.cs              # Populates DB on startup if empty
│   └── Migrations/              # InitialCreate migration
├── Models/
│   ├── Restaurant.cs            # With navigation to MenuItems
│   ├── MenuItem.cs              # decimal macros, IsDirectAdd computed
│   ├── CustomOption.cs
│   └── MenuItemOption.cs        # Junction table with navigation props
├── DTOs/                        # (empty - using anonymous types for now)
├── Endpoints/
│   ├── RestaurantEndpoints.cs   # /restaurants, /restaurants/{id}/menu-items
│   └── MenuItemEndpoints.cs     # /menu-items/{id}, /menu-items/{id}/options
└── FastTrak.Api.csproj          # .NET 9, EF Core 9, Swagger, RateLimit
```

---

## API Endpoint Reference

| Existing Method | REST Endpoint | Notes |
|-----------------|---------------|-------|
| `GetRestaurantsAsync()` | `GET /api/v1/restaurants` | Returns all restaurants |
| `GetMenuItemsForRestaurantAsync(id)` | `GET /api/v1/restaurants/{id}/menu-items` | Paginated, filterable |
| `GetMenuItemAsync(id)` | `GET /api/v1/menu-items/{id}` | Single item with nutrition |
| `GetCustomOptionsForMenuItemAsync(id)` | `GET /api/v1/menu-items/{id}/options` | Available customizations |

---

## Data Acquisition Strategy

**Current:** 4 restaurants (Wendy's, Dunkin', Wingstop, Culver's), 61 menu items, 36 options

**Goal:** Collect menu + nutrition data from all major US fast-food chains (~50+ chains)

**Reality Check:**
- No major chain offers a public API for menu/nutrition data
- Data lives on restaurant websites as HTML pages or downloadable PDFs
- Commercial services like Nutritionix charge $1,850+/month

**Recommended Hybrid Approach:**

| Tier | Source | Use For |
|------|--------|---------|
| 1 | USDA FoodData Central (free API) | Generic foods, backup data |
| 2 | Web scrapers (C# or Python) | Major chains (monthly updates) |
| 3 | Manual PDF extraction | Niche chains, JS-heavy sites |

**Priority Chains for Scraping:**
McDonald's, Chick-fil-A, Taco Bell, Subway, Wendy's, Dunkin', Starbucks, Burger King, Popeyes, Chipotle

**Legal Notes:**
- Nutritional facts are public domain (can't be copyrighted)
- Respect `robots.txt` and rate limits
- Don't republish raw data — add value through customization features

**Next Steps:**
1. Create admin import endpoints (`POST /api/v1/admin/restaurants`, `POST /api/v1/admin/menu-items`)
2. Add API key authentication for admin routes
3. Build first scraper (McDonald's) as proof of concept
4. Schedule monthly updates via Azure Functions

---

## API Assessment Summary (Feb 2026)

**Verdict:** API foundation is solid for MVP. Architecture and endpoint design are correct.

**What's Working:**
- Clean Minimal API pattern with endpoint grouping
- Pagination, category filtering, rate limiting
- Swagger documentation, health endpoint
- Consistent `{ data, meta }` response envelope
- Serilog logging, CORS, security headers

**Remaining Issues (for future work):**
- No HTTP caching (Cache-Control, ETag) — every request hits DB
- No in-memory caching for reference data
- No Application Insights monitoring
- No test project (zero automated tests)
- Anonymous types instead of explicit DTOs

**Full assessment:** See plan file at `.claude/plans/synthetic-mapping-pony.md`

---

## Notes for Future Sessions

When resuming work on this project:
1. Run `git branch` to see current branch
2. Create new branch if starting new work: `git checkout -b feature/your-feature`
3. Review this file for context
4. Check the Session Log above for recent progress
5. Reference the Migration Plan phases
6. Start API before MAUI app: `cd FastTrak.Api && dotnet run --urls "http://*:5050"`
