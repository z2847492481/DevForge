# DevForge

An internal platform for managing **product requirements**, **applications** (frontend apps, backend services, etc.), and **developers** — with traceability between them.

## Tech Stack

- **Frontend v1 (Vue):** Vue 3 + TypeScript + Vite · Element Plus · Pinia · Vue Router
- **Frontend v2 (React):** identical recreation of v1, built after v1 is done (details TBD)
- **Backend:** C# / .NET 10 (LTS) · ASP.NET Core Web API · EF Core · PostgreSQL · Identity + JWT · Serilog

## Status

🚧 Tech stack decided — next step: scaffold the Vue frontend and the .NET backend.

## Roadmap

1. **v1** — Vue 3 frontend + .NET backend (MVP)
2. **v2** — identical React recreation of the frontend
3. **Later** — dashboards, approval workflows, notifications, integrations

## Layout

- `frontend/vue/` — Vue app (version 1, active development)
- `frontend/react/` — React app (version 2, started after the Vue version is complete)
- `backend/` — C# / .NET solution
