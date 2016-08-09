using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abstract { get; set; }
        public string AdditionalDetail { get; set; }
        public string Technologies { get; set; }
        public string Contacts { get; set; }
        public string CodeLink { get; set; }
        public string PreviewLink { get; set; }
        public string AdditionalLinks { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
