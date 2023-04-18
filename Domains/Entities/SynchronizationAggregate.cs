namespace PimsPublisher.Domains.Entities
{
    public class SynchronizationAggregate : Entity<Guid>, IAggregateRoot
    {
        public string ProjectCode { get; set; }
        public string ModelCode { get; set; }
        public int TotalItems { get; set; }
        public int BatchSize { get; set; }
        public int TotalBatch { get; set; }
        public int TotalSubmitted { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IList<SynchronizationBatchEntity> Batches { get; set; }

        private SynchronizationAggregate(string projectCode, string modelCode, int totalItems, DateTime startTime)
            :base(Guid.NewGuid())
        {
            ProjectCode = projectCode;
            ModelCode = modelCode;
            CreatedAt = startTime;
            UpdatedAt = startTime;
            Status = "START";
            TotalItems = totalItems;
            TotalSubmitted = 0;
            BatchSize = 1000;
            Batches = new List<SynchronizationBatchEntity>();
        }

        private void CalculationTotalBatch()
        {
            if (TotalItems > BatchSize) TotalBatch = TotalItems % BatchSize > 0 ? TotalItems / BatchSize + 1 : TotalItems / BatchSize;

            TotalBatch =  1;
        }

        private void CreateSynchronizationBatches()
        {
            for(int batchNo = 0; batchNo < TotalBatch; batchNo++)
            {
                int offset = batchNo * BatchSize;
                int batchTotal = BatchSize;

                if(offset > TotalItems)
                {
                    offset = TotalItems;
                    batchTotal = offset - TotalItems;

                }

                Batches.Add(new SynchronizationBatchEntity(ProjectCode, ModelCode, batchNo, offset, batchTotal, TotalItems)); 
            }
        }
        
        public static SynchronizationAggregate InitNewSynchronizationSession(
        string projectCode,
        string modelCode,
        DateTime startTime, 
            int totalItems)
        {
            var newInstance = new SynchronizationAggregate(projectCode, modelCode, totalItems ,startTime);

            newInstance.CalculationTotalBatch();

            newInstance.CreateSynchronizationBatches();

            return newInstance;
        }
    }
}
