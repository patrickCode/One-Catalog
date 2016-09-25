using System;
using System.Collections.Generic;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;

namespace Microsoft.Catalog.Domain.ProjectContext.Aggregates
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abstract { get; set; }
        public string AdditionalDetails { get; set; }
        public List<Technology> Technologies { get; set; }
        public User CreatedBy { get; set; }
        public List<User> Contacts { get; set; }
        public CodeLink CodeLink { get; set; }
        public PreviewLink PreviewLink { get; set; }
        public List<Link> AdditionalLinks { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public User LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}