using PimsPublisher.Domains.Entities;
using System.Globalization;

namespace PimsPublisher.Infrastructure.K3dClients.DataContract
{
    internal static class MapExtension
    {
        internal static NodeAttribute ToNodeAttribute(this Domains.Entities.NodeAttribute nodeAttributeEntity)
            =>  new NodeAttribute { Key = nodeAttributeEntity.Key, Value = nodeAttributeEntity.Value };
        
        internal static SyncItem ToApiContract(this SynchronizationRecord synchronizationItem)
            => new SyncItem { 
                Identifiers = synchronizationItem.Keys.Select(x => x.ToNodeAttribute()).ToList(),
                Properties = synchronizationItem.Propterties.Select(x => x.ToNodeAttribute()).ToList()
            };

        internal static List<SyncItem> ToApiContractItems(this List<SynchronizationItemEntity> synchronizationItemsOfBatch)
            => synchronizationItemsOfBatch.Select(x => x.SyncRecord.ToApiContract()).ToList();


        internal static SynchSessionBatching ToContractModel(this SynchronizationBatchEntity batchEntity)
        => new SynchSessionBatching 
        { 
            BatchNo = batchEntity.BatchNo,
            BatchTotal = batchEntity.BatchTotal,
            Offset = batchEntity.Offset,
            SessionTotalData = batchEntity.SyncTotal,
            Collections = batchEntity.Items.ToApiContractItems() };

       internal static Domains.Entities.SynchronizationStatus ToSynchronizationStatusEntity(this SynchronizationStatus syncStatus)
            => new Domains.Entities.SynchronizationStatus { Progress = syncStatus.Progress, Status = syncStatus.Status, TotalSubmitted = syncStatus.TotalSumittedData };
    }
}
