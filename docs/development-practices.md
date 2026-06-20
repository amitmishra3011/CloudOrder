# CloudOrder Development Practices

This guide defines the default engineering rules for CloudOrder. Use it when adding features, reviewing pull requests, creating tests, or asking an AI assistant to generate code.

## Architecture Rules

- Keep compile-time dependencies explicit: `RestApi -> Business`; `EFInfrastructure -> Business`; `EFInfrastructure -> Entities`; and `Business -> Entities`.
- The API project (CloudOrder.RestApi) must NOT reference `CloudOrder.EFInfrastructure`. Infrastructure composition (including `AddCloudOrderEFInfrastructure(...)` and `MigrateAndSeedAsync(...)`) must be performed by a host project such as `CloudOrder.Hosting`. API code must not directly use `CloudOrderDbContext`, repository implementations, migrations, or seed classes.
- `CloudOrder.Entities` contains domain entities, value objects, and domain exceptions. It must not reference ASP.NET Core, Entity Framework, infrastructure, or API projects.
- `CloudOrder.EFInfrastructure` owns persistence concerns: `DbContext`, migrations, repository implementations, database configuration, and seed data.
- `CloudOrder.Business` owns use-case orchestration and business services. It also owns repository abstractions needed by business services.
- `CloudOrder.RestApi` owns HTTP concerns: controllers, request/response contracts, validation, exception handling, authentication, authorization, and dependency injection setup.
- `CloudOrder.Worker` is only for background jobs, queue processing, scheduled tasks, and long-running hosted services.
- `CloudOrder.Tests` contains unit and integration tests. Test behavior, not implementation details.

## Coding Standards

- Use clear names that describe business intent: `OrderService`, `IOrderRepository`, `CloudOrderDbContext`.
- Prefer constructor injection. Do not use service locator patterns or manually build service providers inside application code.
- Keep public methods async all the way when they perform I/O. Avoid sync-over-async calls such as `.Result` and `.Wait()`.
- Use `CancellationToken` for new async APIs that may perform I/O or long-running work.
- Return domain or DTO types from business services. Do not leak EF tracking behavior or persistence-specific details into the API layer.
- Keep controllers thin. Controllers should validate inputs, call services, and return HTTP responses.
- Use nullable reference types seriously. Avoid `null!` except where framework initialization requires it.
- Keep comments rare and useful. Explain why a non-obvious decision exists, not what each line does.

## API Rules

- Use RESTful resource names, for example `GET /api/orders` and `GET /api/orders/{id}`.
- Return consistent status codes: `200` for successful reads, `201` for creates, `204` for deletes, `400` for invalid requests, `404` for missing resources, and `500` only for unexpected failures.
- Use centralized exception handling instead of try/catch blocks in every controller.
- Do not expose database entities directly from new API endpoints when the response contract may evolve. Prefer request/response DTOs.
- Validate all external input before it reaches business logic.

## Data Access Rules

- Keep EF Core usage inside `CloudOrder.EFInfrastructure`; `RestApi` may only invoke infrastructure startup extension methods.
- Use repositories for aggregate-level data access. Avoid exposing `IQueryable` outside infrastructure.
- Use migrations for schema changes. Do not manually edit generated migration snapshots unless correcting a known EF generation issue.
- Keep seed data deterministic enough for development and tests.
- Use `AsNoTracking()` for read-only queries unless change tracking is required.

## Testing Rules

- Add or update tests for every business behavior change.
- Unit test business services with mocked dependencies.
- Integration test repository and API behavior when EF queries, routing, serialization, or dependency injection are part of the risk.
- Keep tests readable with arrange, act, assert structure.
- Test names should describe behavior: `GetOrdersAsync_ReturnsOrdersFromRepository`.
- Tests must run with `dotnet test CloudOrder.sln` before merging.

## Security Rules

- Never commit secrets, connection strings for real environments, access keys, tokens, or certificates.
- Keep local-only settings in user secrets or untracked development files.
- Validate and sanitize external input.
- Avoid logging sensitive customer or order data.
- Prefer least-privilege access when adding Azure resources or database permissions.

## Pull Request Checklist

- The solution builds with `dotnet build CloudOrder.sln`.
- Tests pass with `dotnet test CloudOrder.sln`.
- New behavior has appropriate tests.
- Public API changes are documented or visible in Swagger.
- Database schema changes include EF migrations.
- No secrets or machine-specific paths are committed.
- The dependency direction still follows the architecture rules.

## Check-in rules

- Every commit must not break the build or tests. Ensure `dotnet build CloudOrder.sln` and `dotnet test CloudOrder.sln` pass locally before committing.
- A CI workflow runs on push and pull request to build and test the solution. Do not merge PRs with failing checks.
- Optionally enable the local pre-commit hooks included in the repository to run checks automatically before each commit:
  - Set Git to use the provided hooks folder: `git config core.hooksPath .githooks`
  - Make the hooks executable on Unix/macOS: `chmod +x .githooks/pre-commit`

These rules help keep the main branch green and avoid regressions.
