using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Application
{
    public interface IPimsAttributesDataService
    {
        Task<int> GetTotalSynchronizationItem(DateTime startTime, string projectCode, string modelCode);

        Task<List<SynchronizationItem>> ListSynchronizationItems(int offset, int size);
        
    }
    
}