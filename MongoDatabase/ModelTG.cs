using MongoDB.Driver;

namespace MongoDatabase.ModelTG;


public abstract class Document
{
    public Guid Id { get; set; } 
    public long TelegramId { get; set; }
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
    public List<string>? Channels { get; set; } = new();
    public int? Messages { get; set; } = 0;

    public string? LastMessage { get; set; }
    public List<long> Categories { get; set; } = new();
    public List<Channel>? Subscribes { get; set; } 
    public List<Channel>? SubscribesVip { get; set; } 
    public int Attempts { get; set; } = 0;
    public string CurrentPaymentId { get; set; } = "";

    public override void Update()
    {
        UserRepository userRepository = new UserRepository();
        User oldDocument = userRepository.GetDocument(this.Id);
        userRepository.UpdateDocument(oldDocument, this);
    }

}

public class Channel : Document
{
    public Channel() : base("Channel"){}
    public long Owner { get; set; }
    public string Title { get; set; }
    public string? Describtion { get; set; }
    public DateOnly Vip { get; set; } = DateOnly.MinValue;
    public int Reports { get; set; } = 0;
    public override void Update()
    {
        ChannelRepository channelRepository = new ChannelRepository();
        Channel oldDocument = channelRepository.GetDocument(this.Id);
        channelRepository.UpdateDocument(oldDocument, this);
    }
}

public class Category : Document
{
    public Category() : base("Category"){}
    public string Title { get; set; }
    public List<Channel> Channels { get; set; }
    public override void Update()
    {
        CategoryRepository categoryRepository = new CategoryRepository();
        Category oldDocument = categoryRepository.GetDocument(this.Id);
        categoryRepository.UpdateDocument(oldDocument, this);
    }
}