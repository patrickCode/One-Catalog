using System.Collections.Generic;

namespace Microsoft.Catalog.Azure.Search.Interfaces
{
    public interface IDocumentRepository
    {
        void CreateBulk(string indexName, string documentJsonStr);
        void Create(string indexName, Dictionary<string, object> documents);
    }
}