using System;

namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class ChangeHistory
    {
        public DateTime ModifiedOn { get; set; }
        public User ModifiedBy { get; set; }
        public string ChangeSummary { get; set; }
    }
}