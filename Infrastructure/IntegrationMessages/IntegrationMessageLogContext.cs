using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PimsPublisher.Infrastructure.IntegrationMessages
{
    public class IntegrationMessageLogContext : DbContext
    {
        public IntegrationMessageLogContext(DbContextOptions<IntegrationMessageLogContext> options) : base(options)
        {
        }

        public DbSet<IntegrationMessageLogEntry> IntegrationMessageLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IntegrationMessageLogEntry>(ConfigureIntegrationEventLogEntry);
        }

        void ConfigureIntegrationEventLogEntry(EntityTypeBuilder<IntegrationMessageLogEntry> modelBuilder)
        {

                // table
                modelBuilder.ToTable("IntegrationMessageLog");

                // key
                modelBuilder.HasKey(t => t.MessageId);

                // properties
                modelBuilder.Property(t => t.MessageId)
                    .IsRequired()
                    .HasColumnName("MessageId")
                    .HasColumnType("uniqueidentifier");

                modelBuilder.Property(t => t.MessageTypeName)
                    .HasColumnName("MessageTypeName")
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                modelBuilder.Property(t => t.MessageTypeShortName)
                    .HasColumnName("MessageTypeShortName")
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

            modelBuilder.Property(t => t.OccurredTime)
                    .HasColumnName("OccurredTime")
                    .HasColumnType("datetime2");

                modelBuilder.Property(t => t.TimesSent)
                    .HasColumnName("TimesSent")
                    .HasColumnType("int");

                modelBuilder.Property(t => t.Payload)
                    .HasColumnName("PayLoad")
                    .HasColumnType("nvarchar(max)");

                modelBuilder.Property(t => t.State)
                    .HasColumnName("State")
                    .HasColumnType("int");

            modelBuilder.Property(t => t.TransactionId)
                    .HasColumnName("TransactionId")
                    .HasColumnType("nvarchar(256)");

        }
    }
}
