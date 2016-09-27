using Microsoft.Catalog.Azure.Search.Models;
using Microsoft.Catalog.Common.Models.Search;

namespace Microsoft.Catalog.Azure.Search.Interfaces
{
    public interface IAzureSearchContext
    {
        SearchResponse Search(string index, SearchParameters searchParameters);
        SuggestionResponse Suggest(string index, SearchParameters searchParameters);
    }
}