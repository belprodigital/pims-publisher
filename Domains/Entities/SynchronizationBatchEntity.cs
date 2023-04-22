using PimsPublisher.Domains.Events;

namespace PimsPublisher.Domains.Entities
{
    public class SynchronizationBatchEntity: Entity<Guid>
    {
        public Guid SyncId { get; set; }
        public string ProjectCode { get; set; }
        public string ModelCode { get; set; }

        public int BatchNo { get; set; }

        public int Offset { get; set; }

        public int BatchTotal { get; set; }

        public int SyncTotal { get; set; }

        public string JobId { get; set; } = string.Empty;

        public List<SynchronizationItemEntity> Items { get; set; }

        public SynchronizationBatchEntity() : base(Guid.NewGuid()) { }

        private SynchronizationBatchEntity(string projectCode, string modelCode, int batchNo, int offset, int batchTotal, int totalItems):base(Guid.NewGuid())
        {
            ProjectCode = projectCode;
            ModelCode = modelCode;
            BatchNo = batchNo;
            Offset = offset;
            BatchTotal = batchTotal;
            SyncTotal = totalItems;
            Items = new List<SynchronizationItemEntity>();
        }

        public SynchronizationBatchEntity WithDomainEvent()
        {
            AddDomainEvent(SynchronizationBatchCreatedEvent.For(this));

            return this;
        }

        public static SynchronizationBatchEntity For(string projectCode, string modelCode, int batchNo, int offset, int batchTotal, int totalItems)
            => new SynchronizationBatchEntity(projectCode, modelCode, batchNo, offset, batchTotal, totalItems);
    }
}
