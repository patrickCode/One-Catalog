using Microsoft.Catalog.Common.Models.Search;

namespace Microsoft.Catalog.Azure.Search.Interfaces
{
    public interface IAzureSearchContext
    {
        object Search(SearchParameters searchParameters);
    }
}