namespace PimsPublisher.Domains.Entities
{
    internal class SynchronizationBatch
    {
        public Guid Id { get; set; }
        public int BatchNo { get; set; }

        public int Offset { get; set; }

        public int BatchTotal { get; set; }

        public int SessionTotal { get; set; }

        public List<SynchronizationItem>? Collections { get; set; }
    }
}
