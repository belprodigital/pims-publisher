namespace PimsPublisher.Domains.Entities
{
    internal static class BatchCalculationExtension 
    {
        internal static int ToTotalBatchBySize(this int totalItem, int size)
        {
            if(totalItem > size) return totalItem % size > 0 ? totalItem / size +1 : totalItem / size;

            return 1;
        }

    }

    public class SynchronizationSession: Entity, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string ProjectCode { get; set; }
        public string ModelCode { get; set; }
        public int TotalItems { get; set; }
        public int TotalBatch { get; set; }
        public int TotalSubmitted { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        private SynchronizationSession(string projectCode, string modelCode, int totalItems ,DateTime startTime)
        {
            Id = Guid.NewGuid();
            ProjectCode = projectCode;
            ModelCode = modelCode;
            CreatedAt = startTime;
            UpdatedAt = startTime;
            Status = "SENDING";
            TotalItems = totalItems;
            TotalSubmitted = 0;

            TotalBatch = totalItems.ToTotalBatchBySize(1000);
        }

        public static SynchronizationSession InitNewSynchronizationSession(
            string projectCode, 
            string modelCode,
            DateTime startTime, 
            int totalItems)
        {
            var newInstance = new SynchronizationSession(projectCode, modelCode, totalItems ,startTime);

            return newInstance;
        }
    }
}
