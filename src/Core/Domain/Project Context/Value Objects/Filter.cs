namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class Filter
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public object From { get; set; }
        public object To { get; set; }
        public Filter() { }
    }
}