# 🏷️ Product Write-Off and Statistics Tool

A modular **tool focused on: writing off products from stock and viewing monthly statistics of write-offs**, built with:

- 🔧 **ASP.NET Core (C#)** – Backend API & Authentication
- 🌐 **Vue 3 + Vite** – Frontend UI
- 🐘 **PostgreSQL** – Database via Docker
- ⚗️ **xUnit + Moq** – Unit and integration testing
- 🐳 **Docker Compose** – Full system orchestration

---

## 📁 Project Structure

```
.
├── backend/                       # ASP.NET Core backend (Clean Architecture)
│   └── StockManagement.sln
├── frontend/                     # Vue 3 + Vite UI
│   ├── src/                      # Frontend logic
│   └── vite.config.ts
├── docker-compose.yml            # PostgreSQL + backend container
└── Dockerfile                    # Backend build
```

---

## 🚀 Getting Started

### 🔧 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Node.js + npm](https://nodejs.org/)
- Docker + Docker Compose

---

## 🐳 Run With Docker

From the root directory:

```bash
docker-compose up --build
```

This will start:
- `postgres` database on port 5432
- `webapp` (ASP.NET Core API) on port 8080

To connect the frontend manually, set API base to:
```
VITE_API_URL=http://localhost:8080/api/v1
```

---

## 🧰 Frontend (Vue 3)

Navigate to the frontend folder and run:

```bash
npm install          # install dependencies
npm run dev          # run dev server at http://localhost:5173
```

### Frontend

To build for production:

```bash
npm run build
```

---

## ⚙️ Backend (ASP.NET Core)

Apply migrations and run manually:

```bash
dotnet ef database update --project app.dal.ef --startup-project webapp
dotnet run --project webapp
```

Runs backend on:  
📡 `http://localhost:8080/api/v1`

Includes:
- JWT-based authentication (`admin@example.com / Admin123!`)
- Admin role
- CRUD endpoints for inventory, stock, products, suppliers, etc.
- Swagger at `/swagger`

---

## 🧪 Testing

### Backend Unit & Integration Tests

Run tests from root/backend:

```bash
dotnet test
```

Covers:
- Action logic (status update, stock changes)
- Identity flow (login, register, refresh)
- Full API flows

---

## 🔒 Authentication

- Backend uses **JWT Bearer Auth**
- Frontend stores and passes token in requests
- Admin user is seeded on first run

```json
{
  "JWTSecurity": {
    "Key": "...",
    "Issuer": "taltech.ee",
    "Audience": "taltech.ee",
    "ExpiresInSeconds": 86400
  }
}
```

---

## 📄 License

This project is developed for internal company use and is **currently licensed for use only by the author.**
Commercial use, distribution, or public deployment is not permitted without explicit permission.

---

## 🙌 Acknowledgements

- TalTech course project
- Based on Clean Architecture principles