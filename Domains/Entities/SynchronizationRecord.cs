namespace PimsPublisher.Domains.Entities
{
    public class NodeAttribute
    {
        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }

    public class SynchronizationRecord
    {
        public List<NodeAttribute> Keys { get; set; } = new List<NodeAttribute>();
        public List<NodeAttribute> Propterties { get; set; } = new List<NodeAttribute>();
    }
}
