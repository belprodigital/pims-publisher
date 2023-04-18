namespace PimsPublisher.Infrastructure.K3dClients.DataContract
{
    public class SynchronizationStatus
    {
        public Guid SessionId { get; set; }
        public string ProjectCode { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string ModelCode { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public int TotalData { get; set; }
        public int TotalBatch { get; set; }
        public int TotalSumittedData { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
