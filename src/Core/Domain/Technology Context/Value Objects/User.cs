namespace Domain.TechnologyContext.ValueObjects
{
    public class User
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public User() { }
        public User(string alias, string name)
        {
            Alias = alias;
            Name = name;
        }
    }
}