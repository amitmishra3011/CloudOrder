using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CloudOrder.EFInfrastructure.Persistence
{
    public class CloudOrderDbContextFactory : IDesignTimeDbContextFactory<CloudOrderDbContext>
    {
        public CloudOrderDbContext CreateDbContext(string[] args)
        {
            var path = Path.Combine(
             Directory.GetCurrentDirectory(),
             "../CloudOrder.RestApi");

            IConfigurationRoot? configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile(
                    "appsettings.json",
                    optional: false)
                .Build();


            string? connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection");


            var optionsBuilder =
                new DbContextOptionsBuilder<CloudOrderDbContext>();

            optionsBuilder.UseSqlServer(connectionString);


            return new CloudOrderDbContext(
                optionsBuilder.Options);
        }
    }
}
