using System;

namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class Link
    {
        public string LinkType { get; set; }
        public Uri Href { get; set; }
        public string Desciption { get; set; }
        public Link(string linkType, Uri href)
        {
            LinkType = linkType;
            Href = href;
        }
        public Link (string linkType, Uri href, string description)
        {
            LinkType = linkType;
            href = Href;
            Desciption = description;
        }
    }
}