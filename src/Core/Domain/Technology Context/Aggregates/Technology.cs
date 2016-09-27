using System;
using System.Collections.Generic;
using Domain.TechnologyContext.ValueObjects;

namespace Domain.TechnologyContext.Aggregates
{
    public class Technology
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Technology> RelatedTechnologies { get; set; }
        public DateTime CreatedOn { get; set; }
        public User CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public User LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Technology() { }
    }
}