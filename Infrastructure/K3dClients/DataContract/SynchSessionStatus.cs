namespace PimsPublisher.Infrastructure.K3dClients.DataContract
{
    public class SynchSessionStatus
    {
        public Guid SessionId { get; set; }
        public string ProjectCode { get; set; }
        public string ModelCode { get; set; }
        public int TotalData { get; set; }
        public int TotalBatch { get; set; }
        public int TotalSumittedData { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
