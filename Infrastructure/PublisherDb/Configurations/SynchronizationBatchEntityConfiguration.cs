using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Infrastructure.PublisherDb.Configurations
{
    public class SynchronizationBatchEntityConfiguration : IEntityTypeConfiguration<SynchronizationBatchEntity>
    {
        public void Configure(EntityTypeBuilder<SynchronizationBatchEntity> builder)
        {
            builder.ToTable("SynchronizationBatches");

            builder.HasKey(e => new { e.Id });

            builder.Property(e => e.SyncId)
                .IsRequired();

            builder.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.ModelCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.BatchNo)
                .IsRequired();

            builder.Property(e => e.Offset)
                .IsRequired();

            builder.Property(e => e.BatchTotal)
                .IsRequired();

            builder.Property(e => e.SyncTotal)
                .IsRequired();

            builder.HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(e => new { e.BatchId });
        }
    }
}
