using System;

namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class Link
    {
        public int Id { get; set; }
        public string LinkType { get; set; }
        public Uri Href { get; set; }
        public string Desciption { get; set; }
        public Link()
        {
            Id = 0;
        }
        public Link(int id, string linkType, Uri href)
        {
            Id = id;
            LinkType = linkType;
            Href = href;
        }
        public Link(string linkType, Uri href)
        {
            Id = 0;
            LinkType = linkType;
            Href = href;
        }
        public Link (string linkType, Uri href, string description)
        {
            Id = 0;
            LinkType = linkType;
            href = Href;
            Desciption = description;
        }
        public Link(int id, string linkType, Uri href, string description)
        {
            Id = id;
            LinkType = linkType;
            href = Href;
            Desciption = description;
        }
    }
}