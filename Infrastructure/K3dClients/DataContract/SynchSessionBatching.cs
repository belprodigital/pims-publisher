using System.Text.Json.Serialization;

namespace PimsPublisher.Infrastructure.K3dClients.DataContract
{
    public class SynchSessionBatching
    {
        public int BatchNo { get; set; }
        public int Offset { get; set; }
        public int BatchTotal { get; set; }
        public int SessionTotalData { get; set; }
        public List<SyncItem> Collections { get; set; }

    }
}
