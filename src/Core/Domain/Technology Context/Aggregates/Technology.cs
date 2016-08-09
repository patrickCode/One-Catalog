using System;
using System.Collections.Generic;
using Domain.Technology_Context.Value_Objects;

namespace Domain.Technology_Context.Aggregates
{
    public class Technology
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public List<Technology> RelatedTechnologies { get; }
        public DateTime CreatedOn { get; }
        public User CreatedBy { get; }
        public DateTime LastModifiedOn { get; }
        public User LastModifiedBy { get; }
        public bool IsDeleted { get; }
    }
}