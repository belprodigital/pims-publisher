using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Infrastructure.PublisherDb.Configurations
{
    public class SynchronizationAggregateConfiguration : IEntityTypeConfiguration<SynchronizationAggregate>
    {
        public void Configure(EntityTypeBuilder<SynchronizationAggregate> builder)
        {
            // Configure the table name and primary key
            builder.ToTable("Synchronizations");

            builder.HasKey(e => e.Id);

            // Configure the properties
            builder.Property(e => e.ProjectCode).HasMaxLength(16);
            builder.Property(e => e.ModelCode).HasMaxLength(64);
            builder.Property(e => e.TotalItems).IsRequired();
            builder.Property(e => e.BatchSize).IsRequired();
            builder.Property(e => e.TotalBatch).IsRequired();
            builder.Property(e => e.TotalSubmitted).IsRequired();
            builder.Property(e => e.Progress).IsRequired();
            builder.Property(e => e.Status).HasMaxLength(20);
            builder.Property(e => e.CreatedAt).HasColumnType("datetime");
            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");

            // Configure the navigation property
            builder.HasMany(e => e.Batches)
                .WithOne()
                .HasForeignKey(e => new { e.SyncId });
        }
    }

}
