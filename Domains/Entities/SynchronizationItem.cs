namespace PimsPublisher.Domains.Entities
{
    public class NodeAttribute
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
    public class SynchronizationItem
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public int BatchNo { get; set; }

        public List<NodeAttribute> Identifiers { get; set; }
        public List<NodeAttribute> Propterties { get; set; }
    }
}
