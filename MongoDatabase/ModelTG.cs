using MongoDB.Driver;

namespace MongoDatabase.ModelTG;


public abstract class Document
{
    // public long Id { 
    //     get
    //     {
    //         return id;
    //     } 
    //     set
    //     {
    //         id = value;
    //     } 
    // }
    public long DocumentId { get; set; }
    public readonly string Name;
    public DateTime dateTime;

    protected Document(string name)
    {
        dateTime = DateTime.Now;
        Name = name;
    }
    public abstract void Update();

}
public class User : Document
{
    public User() : base("User")
    {
        Channels = new List<string>();
        Categories = new List<long>();
    }
    public string UserName { get; set; }
    public string RefId { get; set; }
    public List<string>? Children { get; set; } = new();
    public List<string>? Channels { get; set; }
    public int? Messages { get; set; } = 0;

    public string? LastMessage { get; set; }
    public List<long>? Categories { get; set; }
    public List<Channel>? Subscribes { get; set; } 
    public List<Channel>? SubscribesVip { get; set; } 
    public int Attempts { get; set; } = 0;
    
    public override void Update()
    {
        UserRepository userRepository = new UserRepository();
        User oldDocument = userRepository.GetDocument(this.DocumentId);
        userRepository.UpdateDocument(oldDocument, this);
    }

}
public abstract class MongoDocument{}
public class Channel : Document
{
    public Channel() : base("Channel"){}
    public long PersonID { get; set; }
    public string Title { get; set; }
    public string Describtion { get; set; }
    public int CategoryID { get; set; }
    public DateOnly Vip { get; set; } = DateOnly.MinValue;
    public int Reports { get; set; } = 0;
    public long AccessHash { get; set; } = 0;
    public override void Update()
    {
        ChannelRepository channelRepository = new ChannelRepository();
        Channel oldDocument = channelRepository.GetDocument(this.DocumentId);
        channelRepository.UpdateDocument(oldDocument, this);
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
        CategoryRepository categoryRepository = new CategoryRepository();
        Category oldDocument = categoryRepository.GetDocument(this.DocumentId);
        categoryRepository.UpdateDocument(oldDocument, this);
    }

}