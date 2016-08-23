using Microsoft.Azure.Search.Models;

namespace Microsoft.Catalog.Azure.Search.Interfaces
{
    public interface IIndexerOperation
    {
        void Run(string indexerName);
        void Reset(string indexerName);
        IndexerExecutionInfo GetLastRunStatus(string indexerName);
    }
}