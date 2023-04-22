using PimsPublisher.Application.Adapters;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Infrastructure.PimsDb
{
    internal class PimsAttributesDataService : IPimsAttributesDataService
    {
        public Task<int> GetTotalSynchronizationItem(DateTime startTime, string projectCode, string modelCode)
        {
            if (projectCode == "0230" && modelCode == "Q0230-shop-str")
            {
                return Task.FromResult(4);
            }
            else
            {
                return Task.FromResult(0);
            }

        }

        public Task<List<SynchronizationRecord>> ListSynchronizationItems(int offset, int size, string projectCode, string modelCode)
        {
            if (projectCode == "0230" && modelCode == "Q0230-shop-str")
            {
                List<SynchronizationRecord> items = new List<SynchronizationRecord>();
                if (offset == 0)
                {
                    items.Add(new SynchronizationRecord
                    {
                        Keys = new List<NodeAttribute> { new NodeAttribute { Key = "Piece Mark", Value = "787053-001-1001-P5PL01" } },
                        Propterties = new List<NodeAttribute> { new NodeAttribute { Key = "CompNo", Value = "CompNo P5PL01" } }
                    });

                    items.Add(new SynchronizationRecord
                    {
                        Keys = new List<NodeAttribute> { new NodeAttribute { Key = "Piece Mark", Value = "787053-001-1001-P5PL02" } },
                        Propterties = new List<NodeAttribute> { new NodeAttribute { Key = "CompNo", Value = "CompNo P5PL02" } }
                    });
                }
                else if (offset == 2)
                {
                    items.Add(new SynchronizationRecord
                    {
                        Keys = new List<NodeAttribute> { new NodeAttribute { Key = "Piece Mark", Value = "787053-001-1001-P5PL03" } },
                        Propterties = new List<NodeAttribute> { new NodeAttribute { Key = "Cut Date", Value = "26-Aug-2022" } }
                    });

                    items.Add(new SynchronizationRecord
                    {
                        Keys = new List<NodeAttribute> { new NodeAttribute { Key = "Piece Mark", Value = "787053-001-1001-P5PL04" } },
                        Propterties = new List<NodeAttribute> { new NodeAttribute { Key = "CompNo", Value = "CompNo P5PL04" } }
                    });
                }

                return Task.FromResult(items);
            }

            return Task.FromResult(new List<SynchronizationRecord>());
        }
    }
}
