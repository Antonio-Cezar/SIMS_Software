# Smart Inventory Management System (SIMS)

A modern, AI-powered inventory management solution for small and medium-sized businesses.  
It provides full visibility into products, stock levels, purchase/sales orders, and automated alerts — with predictive demand forecasting.

---

## Technology Stack

| Layer | Technology | Description |
|-------|-------------|-------------|
| **Frontend (UI)** | [Next.js (React + TypeScript)](https://nextjs.org/) + [shadcn/ui](https://ui.shadcn.com) + [TailwindCSS](https://tailwindcss.com) + [Framer Motion](https://www.framer.com/motion/) | Sleek, responsive web UI with dashboard design, animations, and theming. |
| **Backend (API)** | [ASP.NET Core 9](https://dotnet.microsoft.com/apps/aspnet) + [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) + [PostgreSQL](https://www.postgresql.org/) | REST API for products, inventory, orders, and notifications. |
| **Machine Learning (ML)** | [Python FastAPI](https://fastapi.tiangolo.com/) + [scikit-learn](https://scikit-learn.org/stable/) | Predictive demand forecasting and smart reorder recommendations. |
| **CI/CD & Deployment** | [Docker Compose](https://docs.docker.com/compose/) + [Azure Web App for Containers](https://azure.microsoft.com/en-us/services/app-service/containers/) + GitHub Actions | Containerized environment for development, testing, and cloud deployment. |

---

## 🧠 Key Features

- 🔹 Full CRUD for **products, suppliers, and orders**  
- 🔹 Interactive **dashboard** with KPIs and charts (Recharts)  
- 🔹 **Low-stock alerts** via email or webhooks  
- 🔹 **Machine learning module** (Python) for demand forecasting  
- 🔹 **Authentication & role-based access** (Admin, Manager, Clerk)  
- 🔹 **CI/CD pipeline** with GitHub Actions  
- 🔹 Ready for **Azure deployment**  

---

## Project Structure

```bash
/SIMS_SOFTWARE
  ├── api/            # ASP.NET Core API (C#)
  ├── ml/             # FastAPI (Python) for forecasting
  ├── web/            # Next.js (React + TypeScript) frontend
  ├── docker-compose.yml
  └── README.md
