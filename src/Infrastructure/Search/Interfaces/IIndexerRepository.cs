using System.Collections.Generic;
using Microsoft.Azure.Search.Models;

namespace Microsoft.Catalog.Azure.Search.Interfaces
{
    public interface IIndexerRepository
    {
        Indexer Get(string indexerName);
        List<Indexer> Get();
        bool Exists(string indexerName);
        void Create(string indexerName, string indexerJsonStr, bool force = false);
        void Create(Indexer indexer, bool force = false);
        void Update(string indexerName, string indexerJsonStr, bool createIfNotPresent = true);
        void Delete(string indexerName);
    }
}