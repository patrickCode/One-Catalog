using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Link
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Href { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
