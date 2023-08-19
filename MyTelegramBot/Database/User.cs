
using Telegram.Bot.Types;
namespace MyTelegramBot.Types;

public abstract class Document
{
    public Guid Id { get; set; } 
    public DateTime creationDateTime { get; set; } 

    protected Document()
    {
        creationDateTime = DateTime.Now;
        Id = Guid.NewGuid();
    }

    public abstract void Create();
    public abstract Document Get();
    public abstract Task Update();
    public abstract Task Delete();
}
public class User : Document
{
    public User(long TelegramId, string? UserName = null)
    {
        this.TelegramId = TelegramId;
        this.UserName = UserName;
    }
    public long TelegramId { get; set; }
    public string? UserName { get; set; } 
    public List<Guid> Tarakans { get; set; } = new();
    public string LastMessage { get; set; } = "";

    public override void Create() => Database.CreateDocument(this).Wait();
    public override User Get() => Database.GetDocument<User, Guid>(u => u.Id, this.Id).Result;
    public override async Task Update()
    {
        await this.Delete();
        await Database.CreateDocument(this);
    }
    public override async Task Delete() => await Database.DeleteDocument<User, Guid>(u => u.Id, this.Id);
}
public class Tarakan : Document
{
    public Tarakan(Place place, DateTime detectDateTime)
    {
        this.place = (int)place;
        this.detectDateTime = detectDateTime;
    }
    public Tarakan(Place place)
    {
        this.place = (int)place;
    }
    public DateTime? detectDateTime { get; set; } 
    public int place { get; set; }
    public override void Create() 
    {
        if (Database.GetDocument<User, Guid>(u => u.Id, this.Id).Result == null) Database.CreateDocument(this).Wait();
    }
    public override Tarakan Get() => Database.GetDocument<Tarakan, Guid>(u => u.Id, this.Id).Result;
    public override async Task Update()
    {
        await this.Delete();
        await Database.CreateDocument(this);
    }
    public override async Task Delete() => await Database.DeleteDocument<Tarakan, Guid>(u => u.Id, this.Id);
}


    // private record UserData(long Id, string? UserName);
    // private static UserData GetUserData(Update update)
    // {
    //     // update.CallbackQuery.ChatInstance;
    //     var userData = update.Type switch
    //     {
    //         UpdateType.CallbackQuery => new UserData(update.CallbackQuery.From.Id, update.CallbackQuery.From.Username),
    //         UpdateType.Message => new UserData(update.Message.From.Id, update.Message.From.Username),
    //         _ => throw new NullReferenceException("unable to handle update type")
    //     };
    //     return userData;
    // }
    // public static async Task CreateUser(Update update) 
    // {
    //     var userData = GetUserData(update);
    //     var user = new User(userData.Id, userData.UserName);
    //     await Database.CreateDocument(user);
    // }
    // public static async Task DeleteUser(long Id) 
    //     => await Database.DeleteDocument<User, long>(u => u.TelegramId, Id);
