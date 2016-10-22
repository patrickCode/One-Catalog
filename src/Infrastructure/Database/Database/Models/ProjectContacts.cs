using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Catalog.Database.Models
{
    public class ProjectContact: BaseModel
    {   
        public int ProjectId { get; set; }
        public string Alias { get; set; }
    }

    public class ProjectContactComparer : IEqualityComparer<ProjectContact>
    {
        bool IEqualityComparer<ProjectContact>.Equals(ProjectContact x, ProjectContact y)
        {
            return x.Alias.Equals(y.Alias) && x.ProjectId == y.ProjectId;
        }
        
        int IEqualityComparer<ProjectContact>.GetHashCode(ProjectContact obj)
        {
            return obj.GetHashCode();
        }
    }
}