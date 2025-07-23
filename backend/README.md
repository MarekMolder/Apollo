# StockManagement System

**StockManagement** is a web-based inventory tracking system built with C# and ASP.NET Core.  

It was created as a personal side project to explore backend development, database integration, and clean architectural patterns.

The system is designed to help manage and monitor stock levels in a warehouse environment.

---

## 📦 Project Structure

The backend is organized into modular, clearly separated layers following clean architecture principles:

```
backend/
├── app.bll/               # Business logic implementation
├── app.bll.contracts/     # Interfaces for business logic services
├── app.bll.dto/           # DTOs used within the business layer
├── app.dal.contracts/     # Interfaces for data access
├── app.dal.dto/           # Data access layer DTOs
├── app.dal.ef/            # Entity Framework Core database access implementation
├── app.domain/            # Core domain models and entities
├── app.dto/               # Shared DTOs between layers
├── app.resources/         # Resource files for localization
├── app.tests/             # xUnit test projects for backend logic
├── base.bll/              # Shared business logic base implementations
├── base.bll.contracts/    # Shared interfaces for BLL
├── base.contracts/        # General cross-cutting contracts
├── base.dal.contracts/    # Shared data access interfaces
├── base.dal.ef/           # Shared EF Core support
├── base.domain/           # Base domain models
├── base.helpers/          # Utility classes and helpers
├── webapp/                # ASP.NET Core MVC web frontend
```

This structure ensures clear separation of concerns and makes the system easier to maintain and scale.

---

## 🚀 Getting Started

This project can be run either locally (with manual setup) or fully via Docker.

---

### 🔧 Local Development (without Docker)

#### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/)
- PostgreSQL database running on port 5432
- IDE: Visual Studio / JetBrains Rider / VS Code

#### Steps

```bash
# Clone the repository
git clone https://github.com/your-org/StockManagement.git
cd StockManagement

# Apply EF Core migrations
dotnet ef database update --project app.dal.ef --startup-project webapp

# Run the application
cd webapp
dotnet run
```

Then open your browser at:

- `http://localhost:5000` (HTTP)
- `https://localhost:5001` (HTTPS)

> Ensure your local `appsettings.json` is configured for your database, or use environment variables for `ConnectionStrings__DefaultConnection`.

---

### 🐳 Running with Docker

This project includes a pre-configured `docker-compose.yml` file for full containerized setup.

#### Steps

```bash
# From project root
docker-compose up --build
```

This will start:

- `postgres` database with user/password: `postgres`
- `webapp` accessible at: [http://localhost:8080](http://localhost:8080)

EF Core migrations and data seeding are applied automatically on startup.

#### PostgreSQL Connection

| Setting      | Value         |
|--------------|---------------|
| Host         | `postgres`    |
| Port         | `5432`        |
| Database     | `contactbase` |
| Username     | `postgres`    |
| Password     | `postgres`    |

---

## ✨ Features

The system supports full inventory and stock management, including user roles, data tracking, and workflow approvals.

### ✅ Core Functionalities

- **User authentication and roles**
    - JWT-based login system
    - Admin user is automatically created on first database initialization
    - Admins have full CRUD access to all resources

- **Warehouse & Inventory Management**
    - Manage storage rooms, inventory sessions, and current stock
    - Link products to specific storage rooms and inventories
    - Track and audit all inventory changes

- **Product & Supplier Registry**
    - Create and manage products and categories
    - View and assign suppliers

- **Stock Actions & Approval**
    - Actions (e.g., write-offs, restocks) can be proposed
    - Each action requires approval before updating the current stock
    - Reasons can be attached to stock movements for traceability

- **Auditing & Logging**
    - Inventory audits are supported through `StockAudit` and `StockMovement`
    - All operations are trackable and tied to user actions

### 📊 Managed Entities

- `Product`, `ProductCategory`
- `Supplier`, `Address`
- `CurrentStock`, `Inventory`, `StorageRoom`, `StorageRoomInInventory`
- `StockAudit`, `StockMovement`, `Reason`
- `Action`, `ActionType`, `ApprovalRequest`

---

## 🧰 Tech Stack

This project combines modern backend and frontend technologies for a clean, scalable inventory system.

### 🔙 Backend

- **C# / ASP.NET Core** – RESTful web services & MVC structure
- **Entity Framework Core** – ORM for database access
- **PostgreSQL** – Relational database engine
- **JWT Authentication** – Secure user authentication with roles
- **Swagger** – Interactive API documentation and testing
- **Docker** – Containerized deployment with Docker Compose
- **xUnit** – Unit testing framework for .NET

### 🎨 Frontend

- **Vue.js** – Modern reactive JavaScript framework
- **Bootstrap 5** – Responsive UI design
- **Razor Pages** (optional) – Used internally for some admin views

---

## 🧪 Testing

This project uses **xUnit** for both unit and integration testing to ensure code quality and business rule correctness.

### ✅ Unit Tests

- Located in `app.tests/`
- Cover business logic such as `ActionEntityService`
- Use **Moq** for mocking repositories and mappers
- Focus on behavior of services in isolation

### 🔗 Integration Tests

- Located in `app.tests/Integration/`
- Test full HTTP request/response flows using `HttpClient` and `WebApplicationFactory`
- Include:
    - Full CRUD flow of products, storage rooms, and actions
    - JWT login, refresh, and role-based access
    - Verifying `CurrentStock` updates after action acceptance
- Support seeded admin user (`admin@example.com / Admin123!`)

### 🚀 Run All Tests

To run all tests from root:

```bash
dotnet test
```

Or navigate into the `app.tests/` directory:

```bash
cd app.tests
dotnet test
```

---
> Integration tests are isolated using a custom `TestDbSeeder` and do not affect your real database.

---

## ⚙️ Configuration

All configuration is managed via `appsettings.json`, environment variables, and Docker.

### 🔐 JWT Authentication

Located in `appsettings.json` under `JWTSecurity`:

```json
"JWTSecurity": {
  "Key": "some_super_secret_key_here",
  "Issuer": "taltech.ee",
  "Audience": "taltech.ee",
  "ExpiresInSeconds": 86400,
  "RefreshTokenExpiresInSeconds": 604800
}
```

These values can also be overridden via environment variables:
```
ConnectionStrings__DefaultConnection=
JWTSecurity__Key=
JWTSecurity__Issuer=
...
```

### 🧬 Data Initialization

Database setup behavior on application startup is controlled via:

```json
"DataInitialization": {
  "DropDatabase": false,
  "MigrateDatabase": true,
  "SeedIdentity": true,
  "SeedData": true
}
```

This allows you to choose whether to drop and recreate the database, apply EF migrations, and seed initial data (including the admin user).

### 🌍 Culture & Localization

```json
"DefaultCulture": "et-EE",
"SupportedCultures": [
  "en-GB",
  "et-EE"
]
```

The app supports multilingual views and dynamic language switching.

### 🗃️ Database

Connection string (for PostgreSQL) can be found in:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=contactbase;Username=postgres;Password=postgres"
}
```

In Docker, this is set as:
```
ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=contactbase;Username=postgres;Password=postgres
```

---

> Configuration values can be customized per environment using `appsettings.Development.json` or environment variables in production.

---

## 📄 License

This project is **not licensed for commercial use**.

It is created for **educational purposes**, private experimentation, and portfolio demonstration only.

If you'd like to use parts of this project in your own work, please reach out to the author for permission.