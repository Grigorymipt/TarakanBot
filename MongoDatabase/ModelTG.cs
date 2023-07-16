using MongoDB.Driver;

namespace MongoDatabase.ModelTG;


public abstract class Document
{
    public Guid Id { get; set; }
    public readonly string Name;

    protected Document(string name)
    {
        Name = name;
    }
    public abstract void Update();
        
}
public class User : Document
{
    public User() : base("User")
    {
        Channels = new List<string>();
    }
    public string UserName { get; set; }
    public string RefId { get; set; }
    public List<User>? Children { get; set; } = new();
    public List<string> Channels { get; set; }
    public int? Messages { get; set; } = 0;

    public string? LastMessage { get; set; }
    
    public override void Update()
    {
        UserRepository userRepository = new UserRepository();
        User oldDocument = userRepository.GetDocument(this.Id);
        userRepository.UpdateDocument(oldDocument, this);
    }

}
public abstract class MongoDocument{}
public class Channel : Document
{
    public Channel() : base("Channel"){}
    public Guid PersonID { get; set; }
    public string Title { get; set; }
    public string Describtion { get; set; }
    public int CategoryID { get; set; }
    public override void Update()
    {
        throw new NotImplementedException();
    }
}

public class Category : Document
{
    public Category() : base("Category"){}
    public string ISOTwoLettersCultureCode { get; set; } = "RU";
    public string Title { get; set; }
    public List<Channel> Channels { get; set; }
    public override void Update()
    {
        throw new NotImplementedException();
    }
}