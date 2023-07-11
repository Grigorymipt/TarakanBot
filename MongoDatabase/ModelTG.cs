using MongoDB.Driver;

namespace MongoDatabase.ModelTG;


public class Document
{
    public Guid Id { get; set; }
    public readonly string Name;

    protected Document(string name)
    {
        Name = name;
    }
        
}
public class User : Document
{
    public User() : base("User"){}
    public string? RefId { get; set; }
    public List<User>? Children { get; set; } = new();
    public List<string>? Channels { get; set; }
    public int? Messages { get; set; } = 0;
}
public abstract class MongoDocument{}
public class Channel : Document
{
    public Channel() : base("Channel"){}
    public Guid PersonID { get; set; }
    public string Title { get; set; }
    public string Describtion { get; set; }
    public int CategoryID { get; set; }
}

public class Category : Document
{
    public Category() : base("Category"){}
    public string ISOTwoLettersCultureCode { get; set; } = "RU";
    public string Title { get; set; }
    public List<Channel> Channels { get; set; }
}