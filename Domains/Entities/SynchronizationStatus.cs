namespace PimsPublisher.Domains.Entities
{
    public class SynchronizationStatus
    {
        public int TotalSubmitted { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
    }
}
