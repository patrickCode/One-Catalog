using System.Collections.Generic;

namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class Technology
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Technology> RelatedTechologies { get; set; }
        public Technology() { }
        public Technology(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}