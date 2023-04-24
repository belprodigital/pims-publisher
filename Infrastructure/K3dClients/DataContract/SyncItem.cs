namespace PimsPublisher.Infrastructure.K3dClients.DataContract
{
    public class SyncItem
    {
        public List<NodeAttribute> Identifiers { get; set; }
        public List<NodeAttribute> Properties { get; set; }
    }
}
