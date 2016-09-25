using System.Collections.Generic;

namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class Facet
    {
        public string Name { get; set; }
        public List<Filter> Filters { get; set; }
        public bool IsMultipleAllowed { get; set; }
        public Facet() { }
    }
}