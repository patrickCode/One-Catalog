using System.Collections.Generic;

namespace Microsoft.Catalog.Database.Models
{
    public partial class ProjectTechnologies: BaseModel
    {   
        public int ProjectId { get; set; }
        public int TechnologyId { get; set; }
    }
    public class ProjectTechnologiesComparer : IEqualityComparer<ProjectTechnologies>
    {
        bool IEqualityComparer<ProjectTechnologies>.Equals(ProjectTechnologies x, ProjectTechnologies y)
        {
            return (x.ProjectId == y.ProjectId && x.TechnologyId == y.TechnologyId);
        }

        int IEqualityComparer<ProjectTechnologies>.GetHashCode(ProjectTechnologies obj)
        {
            return obj.GetHashCode();
        }
    }
}