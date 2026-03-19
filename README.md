# AiSqlEngine

An ASP.NET Core Web API server that allows authenticated users to convert **natural language (NLP) queries into SQL**, validate the generated SQL, and execute it against a relational database — all via a clean REST API.

Authentication is handled through **Veracity (OpenID Connect)**, and SQL generation is powered by **Azure OpenAI**.

---

## Features

- **Authentication** — Sign in / sign out via Veracity (OpenID Connect + cookie-based sessions). Retrieve current user info.
- **NLP → SQL** — Send a natural language message and receive a validated SQL query back.
- **SQL Execution** — Execute a SQL query (with validation) and receive the results.
- **Schema-aware generation** — The LLM prompt is dynamically enriched with the actual database schema for accurate query generation.

---

## Architecture

The solution is split into four projects:

| Project | Responsibility |
|---|---|
| `AiSqlEngine` | Host / startup (ASP.NET Core entry point) |
| `AiSqlEngine.Api` | Controllers, dependency registration for the API layer |
| `AiSqlEngine.Core` | Domain interfaces, models, prompt building, query orchestration |
| `AiSqlEngine.Infrastructure` | Azure OpenAI client, SQL executor, schema tool, validators |

---

## API Endpoints

### Authentication — `/api/auth`

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/auth/SignIn` | Initiates OpenID Connect login flow |
| `GET` | `/api/auth/SignOut` | Signs out the current user (requires auth) |
| `GET` | `/api/auth/me` | Returns info about the currently authenticated user |

### Query — `/api/query`

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/query/generate-sql` | Converts a natural language message to a validated SQL string |
| `POST` | `/api/query/execute` | Validates and executes a SQL query, returns results |

#### `POST /api/query/generate-sql`

**Request body:**
```json
{
  "message": "Show me all customers from Norway who placed an order in 2024"
}
```

**Response:**
```json
{
  "sql": "SELECT * FROM Customers c JOIN Orders o ON c.Id = o.CustomerId WHERE c.Country = 'Norway' AND YEAR(o.OrderDate) = 2024"
}
```

#### `POST /api/query/execute`

**Request body:**
```json
{
  "sql": "SELECT * FROM Customers WHERE Country = 'Norway'"
}
```

**Response:** JSON array of result rows.

---

## Configuration

Configure the following in `appsettings.json` / `appsettings.Development.json` or environment variables:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<your-sql-connection-string>"
  },
  "OpenApi": {
    "Endpoint": "<azure-openai-endpoint>",
    "ApiKey": "<azure-openai-api-key>",
    "Deployment": "<deployment-name>"
  },
  "Veracity": {
    "ClientId": "<client-id>",
    "ClientSecret": "<client-secret>",
    "TenantId": "<tenant-id>",
    "MyServicesApi": "<veracity-my-services-api-url>"
  }
}
```

> **Never commit secrets.** Use environment variables, Azure Key Vault, or the .NET Secret Manager for local development.

---

## Getting Started

**Prerequisites:** .NET 10 SDK, access to Azure OpenAI, a SQL Server database, and a Veracity application registration.

```bash
# Restore dependencies
dotnet restore

# Run the API
dotnet run --project AiSqlEngine
```

Swagger UI is available at `https://localhost:<port>/swagger` when running in development mode.

---

## Tech Stack

- ASP.NET Core 10
- Azure OpenAI (`Azure.AI.OpenAI`)
- Veracity Authentication (OpenID Connect)
- FluentResults
- Newtonsoft.Json

