# CloudOrder

CloudOrder is a cloud-native Order Management Platform built using **.NET 8**, **Clean Architecture**, **Entity Framework Core**, and modern ASP.NET Core practices. The project is designed as a reference implementation for building scalable, maintainable, and production-ready REST APIs.

---

# Features

- Clean Architecture
- RESTful APIs
- Entity Framework Core
- FluentValidation
- AutoMapper
- Global Exception Handling using Problem Details
- OpenAPI support
- Scalar API Reference
- Sample data seeding
- Unit Tests

---

# Solution Structure

```
CloudOrder
│
├── src
│   ├── CloudOrder.Api
│   ├── CloudOrder.Business
│   ├── CloudOrder.Entities
│   └── CloudOrder.Infrastructure
│
├── tests
│   └── CloudOrder.Tests
│
└── docs
    └── development-practices.md
```

---

# Prerequisites

Install the following before running the solution:

- .NET 10 SDK
- Visual Studio 2026 (18.10 or later) or Visual Studio Code
- Git

Verify your installation:

```bash
dotnet --version
```

---

# Clone the Repository

```bash
git clone <repository-url>
cd CloudOrder
```

---

# Restore NuGet Packages

```bash
dotnet restore
```

---

# Build the Solution

```bash
dotnet build CloudOrder.sln
```

---

# Run Unit Tests

```bash
dotnet test CloudOrder.sln
```

---

# Running the Application

From the solution root:

```bash
dotnet run --project src/CloudOrder.Api
```

Alternatively, open the solution in Visual Studio and press **F5**.

During startup the application automatically:

- Creates or updates the database (if configured)
- Seeds sample Customers
- Seeds sample Products
- Seeds sample Orders

No manual database setup is required for local development.

---

# Finding the Application URL

When the application starts successfully, the console displays something similar to:

```text
info: Microsoft.Hosting.Lifetime[14]

Now listening on:

https://localhost:7150
http://localhost:5197
```

Your port number may be different.

Use the HTTPS URL whenever possible.

Example:

```
https://localhost:7150
```

---

# OpenAPI Documentation

CloudOrder uses the built-in ASP.NET Core OpenAPI support.

Open the following URL in your browser:

```
https://localhost:7150/openapi/v1.json
```

Replace **7150** with the port shown in your console.

This endpoint returns the OpenAPI specification in JSON format.

---

# Scalar API Reference

CloudOrder includes Scalar for interactive API documentation.

Open:

```
https://localhost:7150/scalar
```

Replace **7150** with your application's HTTPS port.

Using Scalar you can:

- Browse all API endpoints
- View request and response models
- Understand endpoint parameters
- Execute requests directly from the browser

---

# Available API Endpoints

## Customers

| Method | Endpoint |
|---------|----------|
| GET | `/api/customers` |
| GET | `/api/customers/{id}` |
| POST | `/api/customers` |
| PUT | `/api/customers/{id}` |
| DELETE | `/api/customers/{id}` |

## Orders

| Method | Endpoint |
|---------|----------|
| GET | `/api/orders` |
| GET | `/api/orders/{id}` |
| POST | `/api/orders` |

---

# Development

Please read the project development guidelines before contributing.

- Project standards: `docs/development-practices.md`

Common commands:

```bash
dotnet restore
dotnet build CloudOrder.sln
dotnet test CloudOrder.sln
```

---

# Troubleshooting

## Cannot access the API

Ensure the application is running.

Verify the console output contains:

```text
Now listening on:
https://localhost:7150
```

Use the displayed port number instead of the example port.

---

## OpenAPI returns 404

Verify the following configuration exists in `Program.cs`:

```csharp
builder.Services.AddOpenApi();

...

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
```

---

## Scalar returns 404

Ensure the Scalar package is installed and configured:

```csharp
app.MapScalarApiReference();
```

---

## HTTPS Certificate Warning

If the browser reports an untrusted development certificate, run:

```bash
dotnet dev-certs https --trust
```

---

# Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- AutoMapper
- FluentValidation
- OpenAPI
- Scalar
- xUnit