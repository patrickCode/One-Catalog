using System.Collections.Generic;
using Microsoft.Azure.Search.Models;

namespace Microsoft.Catalog.Azure.Search.Interfaces
{
    public interface IIndexRepository
    {
        Index Get(string indexName);
        List<Index> Get();
        bool Exists(string indexName);
        void Create(string indexName, string indexJsonStr, bool force = false);
        void Create(Index index, bool force = false);
        void Update(string indexName, string indexJsonStr, bool createIfNotPresent);
        void Delete(string indexName);
    }
}