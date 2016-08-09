using System;
using System.Collections.Generic;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;

namespace Microsoft.Catalog.Domain.ProjectContext.Aggregates
{
    public class Project
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string Abstract { get; }
        public string AdditionalDetails { get; }
        public List<Technology> Technologies { get; }
        public User CreatedBy { get; }
        public List<User> Contacts { get; }
        public CodeLink CodeLink { get; }
        public PreviewLink PreviewLink { get; }
        public List<Link> AdditionalLinks { get; }

        public DateTime CreatedOn { get; }
        public DateTime LastModifiedOn { get; }
        public User LastModifiedBy { get; }
        public bool IsDeleted { get; }
    }
}
