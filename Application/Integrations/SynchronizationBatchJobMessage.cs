using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Application.Integrations
{
    public class SynchronizationBatchJobMessage : IJobMessage
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public DateTime Occurred { get; set; }

        public Guid BatchId { get; set; }
        public Guid SyncId { get; set; }
        public string ProjectCode { get; set; }
        public string ModelCode { get; set; }
        public int BatchNo { get; set; }
        public int Offset { get; set; }
        public int BatchTotal { get; set; }
        public int SyncTotal { get; set; }

        public SynchronizationBatchJobMessage() { }


        private SynchronizationBatchJobMessage(SynchronizationBatchEntity batchEntity)
        {
            Id = Guid.NewGuid();
            Occurred = DateTime.Now;
            EntityId = batchEntity.Id;

            BatchId = batchEntity.Id;
            SyncId = batchEntity.SyncId;
            ProjectCode = batchEntity.ProjectCode;
            ModelCode = batchEntity.ModelCode;
            BatchNo = batchEntity.BatchNo;
            Offset = batchEntity.Offset;
            BatchTotal = batchEntity.BatchTotal;
            SyncTotal = batchEntity.SyncTotal;
        }

        public static SynchronizationBatchJobMessage For(SynchronizationBatchEntity batchEntity)
        {
            return new SynchronizationBatchJobMessage(batchEntity);
        }
    }
}
