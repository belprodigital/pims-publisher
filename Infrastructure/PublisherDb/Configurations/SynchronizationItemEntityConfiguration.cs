using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PimsPublisher.Domains.Entities;
using Newtonsoft.Json;

namespace PimsPublisher.Infrastructure.PublisherDb.Configurations
{

    public class SynchronizationItemEntityConfiguration : IEntityTypeConfiguration<SynchronizationItemEntity>
    {
        public void Configure(EntityTypeBuilder<SynchronizationItemEntity> builder)
        {
            builder.ToTable("SynchronizationItems");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("ItemId");

            builder.Property(e => e.BatchId)
                .IsRequired();

            builder.Property(e => e.SyncId)
                .IsRequired();

            builder.Property(e => e.BatchNo)
                .IsRequired();

            builder.OwnsOne(e => e.SyncRecord, record =>
            {
                record.Property(e => e.Keys)
                    .HasColumnName("Keys")
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                        v => JsonConvert.DeserializeObject<List<NodeAttribute>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

                record.Property(e => e.Propterties)
                    .HasColumnName("Properties")
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                        v => JsonConvert.DeserializeObject<List<NodeAttribute>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            });
        }
        
    }
}


