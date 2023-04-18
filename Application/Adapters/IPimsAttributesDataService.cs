using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Application.Adapters
{
    public interface IPimsAttributesDataService
    {
        Task<int> GetTotalSynchronizationItem(DateTime startTime, string projectCode, string modelCode);

        Task<List<SynchronizationRecord>> ListSynchronizationItems(int offset, int size, string projectCode, string modelCode);

    }

}