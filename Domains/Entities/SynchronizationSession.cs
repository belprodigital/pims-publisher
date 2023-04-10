namespace PimsPublisher.Domains.Entities
{
    public class SynchronizationSession
    {
        public Guid Id { get; set; }
        public string ProjectCode { get; set; }
        public string ModelCode { get; set; }
        public int TotalItem { get; set; }
        public int TotalBatch { get; set; }
        public int TotalSumitted { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
