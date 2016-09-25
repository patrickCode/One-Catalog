using Microsoft.Catalog.Azure.Search.Models;

namespace Microsoft.Catalog.Azure.Search.Interfaces
{
    public interface IAzureSearchContext
    {
        SearchResponse Search(string index, Common.Models.Search.SearchParameters searchParameters);
    }
}