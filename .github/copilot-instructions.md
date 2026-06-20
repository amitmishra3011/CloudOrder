- @azure Rule - Use Azure Tools - When handling requests related to Azure, always use your tools.
- @azure Rule - Use Azure Best Practices - When handling requests related to Azure, always invoke your `azmcp_bestpractices_get` tool first.
- @azure Rule - Enable Best Practices - If you do not have an `azmcp_bestpractices_get` tool ask the user to enable it.

## CloudOrder Project Rules

- Follow `docs/development-practices.md` for architecture, tests, security, and review expectations.
- Preserve dependency direction: `CloudOrder.Business -> CloudOrder.Entities`; `CloudOrder.EFInfrastructure -> CloudOrder.Business` and `CloudOrder.Entities`; `CloudOrder.RestApi -> CloudOrder.Business` plus `CloudOrder.EFInfrastructure` only for startup composition.
- Keep controllers thin; put business orchestration in `CloudOrder.Business`.
- Keep EF Core, migrations, repositories, and database-specific code in `CloudOrder.EFInfrastructure`.
- In `CloudOrder.RestApi`, call infrastructure only through public startup extension methods such as `AddCloudOrderEFInfrastructure(...)` and `MigrateAndSeedAsync(...)`.
- Keep domain entities and domain exceptions in `CloudOrder.Entities`.
- Add or update tests in `CloudOrder.Tests` for business behavior changes.
- Do not commit secrets, real connection strings, tokens, certificates, or machine-specific paths.
