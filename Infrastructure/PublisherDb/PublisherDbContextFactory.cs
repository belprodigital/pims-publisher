using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PimsPublisher.Infrastructure.PublisherDb
{
    public class PublisherDbContextFactory : IDesignTimeDbContextFactory<PublisherDbContext>
    {
        public PublisherDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("migration.appsettings.json")
               .Build();

            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine(configuration.GetConnectionString("publisherDbConnectionString"));


            var optionsBuilder = new DbContextOptionsBuilder<PublisherDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("publisherDbConnectionString"));

            return new PublisherDbContext(optionsBuilder.Options);
        }
    }
}
