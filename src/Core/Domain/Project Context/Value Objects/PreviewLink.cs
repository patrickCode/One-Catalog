using System;

namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class PreviewLink: Link
    {
        public PreviewLink(): base() { }
        public PreviewLink(Uri href): base("Preview", href) { }
        public PreviewLink(Uri href, string description): base("Preview", href, description) { }
    }
}