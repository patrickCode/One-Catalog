using System;

namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class CodeLink: Link
    {
        public CodeLink(): base() { }
        public CodeLink(Link link): base(link.Id, "Code", link.Href, link.Desciption) { }
        public CodeLink(Uri href): base("Code", href) { }
        public CodeLink(Uri href, string description) : base("Code", href, description) { }
    }
}