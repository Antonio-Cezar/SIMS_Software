# Smart Inventory Management System (SIMS)

A modern, modular inventory management system for small and medium-sized businesses.  
Currently under development — providing product management, inventory tracking, and usage reports.

---

## Technology Stack

| Layer | Technology | Description |
|-------|-------------|-------------|
| **Frontend (UI)** | HTML + CSS + JavaScript | Simple and responsive interface for viewing dashboard, products, orders, and reports. |
| **Backend (API)** | [ASP.NET Core 9](https://dotnet.microsoft.com/apps/aspnet) + [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) + [SQLite](https://www.sqlite.org/) | REST API handling users, inventory, and order data. |
| **Authentication** | Custom JWT-based authentication | Lightweight login system without full Identity. |
| **Future ML Integration** | Python (FastAPI + scikit-learn) *(planned)* | Will power demand forecasting and automated reordering. |
| **Deployment** | [Docker Compose](https://docs.docker.com/compose/) *(planned)* | Containerized setup for local and future cloud deployment. |

---

## Key Features (Current Phase)

- ✅ **User login & JWT authentication**
- ✅ **Dashboard view** (store info & basic statistics)
- ✅ **Product management** (view current stock)
- ✅ **Orders view** (shows what to order for next week)
- ✅ **Reports tab** (weekly/monthly usage overview)
- 🔄 **SQLite database integration**
- 🚧 **AI forecasting** *(coming soon)*

---

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
  │   ├── frontend/                         # Frontend (HTML, CSS, JavaScript)
  │   │   ├── index.html
  │   │   ├── script.js
  │   │   ├── styles.css
  │   │   └── images/
  │   ├── python/                           # Python scripts or experiments
  │   └── SmartInventory.Api/               # ASP.NET Core Web API (C# backend)
  │       ├── bin/
  │       ├── Data/                         # Database context and seed data
  │       ├── Migrations/
  │       ├── Models/                       # Data models and entities
  │       ├── obj/
  │       ├── Properties/
  │       ├── appsettings.json              # Main application settings
  │       ├── appsettings.Development.json  # Dev-specific settings
  │       ├── inventory.db*                 # SQLite database files
  │       ├── Program.cs                    # Application entry point
  │       ├── SmartInventory.Api.csproj     # C# project file
  │       └── SmartInventory.Api.http
  ├── README.md
  └── SIMS_Software.sln                     # Visual Studio solution file
```

## Project Updates

![Layout](/SIMS_MVP/images/Layout%20mock.jpg)

---

![First look](/SIMS_MVP/images/updates%201.jpg.jpg)

---
