using System.Collections.Generic;

namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class FacetInfo
    {
        public string Name { get; set; }
        public List<Filter> Filters { get; set; }
        public FacetInfo() { }
    }
}