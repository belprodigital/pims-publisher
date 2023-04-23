using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PimsPublisher.Infrastructure.IntegrationMessages
{
    public class IntegrationMessageLogContextFactory : IDesignTimeDbContextFactory<IntegrationMessageLogContext>
    {
        public IntegrationMessageLogContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("migration.appsettings.json")
              .Build();

            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine(configuration.GetConnectionString("publisherDbConnectionString"));


            var optionsBuilder = new DbContextOptionsBuilder<IntegrationMessageLogContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("publisherDbConnectionString"));

            return new IntegrationMessageLogContext(optionsBuilder.Options);
        }
    }
}
