using MongoDatabase;
using MongoDatabase.ModelTG;
using MyTelegramBot.Convertors;
using MyTelegramBot.Utils;
using Telegram.Bot.Types;
using User = MongoDatabase.ModelTG.User;
namespace MyTelegramBot.Types;

public static class Database
{
    static CommandParser ArgumentParser = new CommandParser();

    public static User CreateUser(Message message)
    {
        var collection = new UserRepository();
        var parent = collection.GetDocument(
            ArgumentParser.Parse(message.Text).ArgumentsText);
        
        var user = new User()
        {
            Id = IdConvertor.ToGuid(message.From.Id),
            UserName = message.From.Username,
            Channels = new List<string>(),
            Categories = new List<Guid>(),
            Children = new List<string>(),
        };
        if (parent != null)
        {
            Console.WriteLine(parent.UserName);
            string parentUserName = parent.UserName;
            user.RefId = parentUserName;
            if (parent.Children == null) 
                parent.Children = new List<string>() { user.UserName };
            else
                parent.Children.Add(user.UserName);
            parent.Update();
        }
        else user.RefId = null;
        collection.CreateDocument(user);
        return user;
    }
    /// <returns>The session of the sender of a given <c>Message</c> object.</returns>
    public static async Task<User> GetUserAsync(long Id)
    {
        var collection = new UserRepository();
        User user = await collection.GetDocumentAsync(IdConvertor.ToGuid(Id));
        return user;
    }
    public static User GetUser(long Id)
    {
        var collection = new UserRepository();
        var user = collection.GetDocument(IdConvertor.ToGuid(Id));
        return user;
    }
    public static User GetUser(string username)
    {
        var collection = new UserRepository();
        var user = collection.GetDocument(username);
        return user;
    }
    public static User UpdateUser(Message message)
    {
        var collection = new UserRepository();
        Console.WriteLine(message.Text);
        var parent = collection.GetDocument(ArgumentParser.Parse(message.Text).ArgumentsText);
        var user = GetUser(message.From.Id);
        if (parent != null)
        {
            string parentUserName = parent.UserName;
            user.RefId = parentUserName;
        }
        else user.RefId = null;
        user.Update();
        return user;
    }

    public static void DeleteUser(long Id)
    {
        var collection = new UserRepository();
        var user = GetUser(Id);
        collection.DeleteDocumentAsync(user);
    }
    public static void DeleteUser(User user)
    {
        var collection = new UserRepository();
        collection.DeleteDocumentAsync(user);
    }
    
    
    public static void CreateCategory(Message message)
    {
        var messageArgs = ArgumentParser.Parse(message.Text).ArgumentsText;
        CreateCategory(messageArgs);
    }
    private static void CreateCategory(string Title)
    {
        var collection = new CategoryRepository();
        
        var category = new Category()
        {
            Id = new Guid(),
            Title = Title
        };
        collection.CreateDocument(category);   
    }
    public static async Task<Category> GetCategoryAsync(Guid Id)
    {
        var collection = new CategoryRepository();
        var category = await collection.GetDocumentAsync(Id);
        if (category == null) CreateCategory("This category is in progress of creation.");
        return category;
    }
    public static async Task<Category> GetCategoryAsync(Message message)
    {
        var Id = ArgumentParser.Parse(message.Text).ArgumentsText;
        var category = await GetCategoryAsync(new Guid(Id));
        return category;
    }
    public static async Task<List<Category>> GetAllCategories()
    {
        var collection = new CategoryRepository();
        return await collection.GetAllDocumentsAsync();
    }

    public static void CreateChannel(Channel channel)
    {
        var collection = new ChannelRepository();
        collection.CreateDocument(channel);
    }
    public static void CreateChannel(string Title)
    {
        var collection = new ChannelRepository();
        Title.Remove('@');
        var channel = new Channel()
        {
            Id = new Guid(),
            Title = Title
        };
        collection.CreateDocument(channel);
    }
    
    public static Channel GetChannel(Message message)
    {
        var collection = new ChannelRepository();
        var channel = collection.GetDocument(ArgumentParser.Parse(message.Text).ArgumentsText);
        return channel;
    }
    public static Channel GetChannel(string name)
    {
        var collection = new ChannelRepository();
        var channel = collection.GetDocument(name);
        return channel;
    }
    public static async Task<Channel> GetChannelAsync(Guid Id)
    {
        var collection = new ChannelRepository();
        var channel = await collection.GetDocumentAsync(Id);
        if (channel == null) CreateCategory("This channel is in progress of creation.");
        return channel;
    }
    public static async Task<Channel> GetChannelAsync(Message message)
    {
        var Id = ArgumentParser.Parse(message.Text).ArgumentsText;
        var channel = await GetChannelAsync(new Guid(Id));
        return channel;
    }
    public static async Task<List<Channel>> FindChannelToListAsync()
    {   
        var collection = new ChannelRepository();
        List<Channel> channels = await collection.GetOldestDocuments(20);
        return channels;
    }
}