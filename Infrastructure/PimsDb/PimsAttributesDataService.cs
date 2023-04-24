using PimsPublisher.Application.Adapters;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Infrastructure.PimsDb
{
    
    internal class PimsAttributesDataService : IPimsAttributesDataService
    {
        private readonly List<SynchronizationRecord> items = new List<SynchronizationRecord>
        {
            new SynchronizationRecord
            {
                Keys = new List<NodeAttribute> { new NodeAttribute { Key = "Piece Mark", Value = "787053-001-1001-P5PL01" } },
                Propterties = new List<NodeAttribute> { new NodeAttribute { Key = "CompNo", Value = "CompNo P5PL01" } }
            },
            new SynchronizationRecord
            {
                Keys = new List<NodeAttribute> { new NodeAttribute { Key = "Piece Mark", Value = "787053-001-1001-P5PL02" } },
                Propterties = new List<NodeAttribute> { new NodeAttribute { Key = "CompNo", Value = "CompNo P5PL02" } }
            },
           new SynchronizationRecord
            {
                Keys = new List<NodeAttribute> { new NodeAttribute { Key = "Piece Mark", Value = "787053-001-1001-P5PL03" } },
                Propterties = new List<NodeAttribute> { new NodeAttribute { Key = "Cut Date", Value = "26-Aug-2022" } }
            },
           new SynchronizationRecord
            {
                Keys = new List<NodeAttribute> { new NodeAttribute { Key = "Piece Mark", Value = "787053-001-1001-P5PL04" } },
                Propterties = new List<NodeAttribute> { new NodeAttribute { Key = "CompNo", Value = "CompNo P5PL04" } }
            }
        };
        public Task<int> GetTotalSynchronizationItem(DateTime startTime, string projectCode, string modelCode)
        {
            return Task.FromResult(items.Count());

        }

        public Task<List<SynchronizationRecord>> ListSynchronizationItems(int offset, int size, string projectCode, string modelCode)
        {
            return Task.FromResult(items.Skip(offset).Take(size).ToList());
        }
    }
}
