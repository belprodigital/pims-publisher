namespace PimsPublisher.Domains.Entities
{
    public class SynchronizationItemEntity : Entity<Guid>
    {
        public Guid BatchId { get; set; }
        public Guid SyncId { get; set; }
        public int BatchNo { get; set; }

        public SynchronizationRecord SyncRecord { get; set; }

        private SynchronizationItemEntity(Guid syncId, Guid batchId, int batchNo, SynchronizationRecord record) : base(Guid.NewGuid())
        {
            SyncId = syncId;
            BatchNo = batchNo;
            SyncRecord = record;
            BatchId = batchId;
        }

        public static SynchronizationItemEntity For(Guid syncId, Guid batchId, int batchNo, SynchronizationRecord item)
            => new SynchronizationItemEntity(syncId, batchId, batchNo, item);

    }
}
