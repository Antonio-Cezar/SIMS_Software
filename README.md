# Smart Inventory Management System (SIMS)

A modern, AI-powered inventory management solution for small and medium-sized businesses.  
It provides full visibility into products, stock levels, purchase/sales orders, and automated alerts.

---

## Technology Stack

| Layer | Technology | Description |
|-------|-------------|-------------|
| **Frontend (UI)** | [Next.js (React + TypeScript)](https://nextjs.org/) + [shadcn/ui](https://ui.shadcn.com) + [TailwindCSS](https://tailwindcss.com) + [Framer Motion](https://www.framer.com/motion/) | Sleek, responsive web UI with dashboard design, animations, and theming. |
| **Backend (API)** | [ASP.NET Core 9](https://dotnet.microsoft.com/apps/aspnet) + [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) + [PostgreSQL](https://www.postgresql.org/) | REST API for products, inventory, orders, and notifications. |
| **Machine Learning (ML)** | [Python FastAPI](https://fastapi.tiangolo.com/) + [scikit-learn](https://scikit-learn.org/stable/) | Predictive demand forecasting and smart reorder recommendations. |
| **CI/CD & Deployment** | [Docker Compose](https://docs.docker.com/compose/) + [Azure Web App for Containers](https://azure.microsoft.com/en-us/services/app-service/containers/) + GitHub Actions | Containerized environment for development, testing, and cloud deployment. |

---

## Key Features

- 🔹 Full CRUD for **products, suppliers, and orders**  
- 🔹 Interactive **dashboard** with KPIs and charts (Recharts)  
- 🔹 **Low-stock alerts** via email or webhooks  
- 🔹 **Machine learning module** (Python) for demand forecasting  
- 🔹 **Authentication & role-based access** (Admin, Manager, Clerk)  
- 🔹 **CI/CD pipeline** with GitHub Actions  
- 🔹 Ready for **Azure deployment**  

---
## Project testing run on WEB (vs code terminal)
Run from SmartInventory.Api/

```bash
dotnet run
```
---

Run from frontend/ :

```bash
npx http-server -p 8080
```
---

## Project Structure

```bash
/SIMS_SOFTWARE
  ├── SIMS_MVP/
  │   ├── frontend/                # Frontend (HTML, CSS, JavaScript)
  │   │   ├── index.html
  │   │   ├── script.js
  │   │   ├── styles.css
  │   │   └── images/
  │   ├── python/                 # Python scripts or experiments
  │   └── SmartInventory.Api/     # ASP.NET Core Web API (C# backend)
  │       ├── bin/
  │       ├── Data/               # Database context and seed data
  │       ├── Migrations/
  │       ├── Models/             # Data models and entities
  │       ├── obj/
  │       ├── Properties/
  │       ├── appsettings.json    # Main application settings
  │       ├── appsettings.Development.json # Dev-specific settings
  │       ├── inventory.db*       # SQLite database files
  │       ├── Program.cs          # Application entry point
  │       ├── SmartInventory.Api.csproj # C# project file
  │       └── SmartInventory.Api.http
  ├── README.md
  └── SIMS_Software.sln           # Visual Studio solution file


```