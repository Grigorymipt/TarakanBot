using MongoDatabase;
using MongoDatabase.ModelTG;
using MyTelegramBot.Convertors;
using MyTelegramBot.Utils;
using Serilog;
using Telegram.Bot.Types;
using User = MongoDatabase.ModelTG.User;
namespace MyTelegramBot.Types;

public class Database
{
    static readonly CommandParser ArgumentParser = new();
    readonly static UserRepository _userRepository = new();
    readonly static CategoryRepository _categoryRepository = new();
    readonly static ChannelRepository _channelRepository = new();

    public static User CreateUser(Message message)
    {
        var _userRepository = new UserRepository();
        var parent = _userRepository.GetDocument(
            ArgumentParser.Parse(message.Text).ArgumentsText);
        
        var user = new User()
        {
            TelegramId = message.From.Id,
            UserName = message.From.Username,
            Channels = new List<string>(),
            Categories = new List<long>(),
            Children = new List<string>(),
        };
        if (parent != null && parent.TelegramId != user.TelegramId)
        {
            Console.WriteLine(parent.UserName);
            Log.Information(parent.UserName);
            string parentUserName = parent.UserName;
            user.RefId = parentUserName;
            if (parent.Children == null) 
                parent.Children = new List<string>() { user.UserName };
            else
                parent.Children.Add(user.UserName);
            parent.Update();
        }
        else user.RefId = null;
        _userRepository.CreateDocument(user);
        return user;
    }
    /// <returns>The session of the sender of a given <c>Message</c> object.</returns>
    public static async Task<User> GetUserAsync(long Id) { return await _userRepository.GetDocumentAsync(Id);}

    public static User GetUser(long Id) { return _userRepository.GetDocument(Id);}
    public static User GetUser(string username){ return _userRepository.GetDocument(username);
    }
    public static User GetUserByTrnId(string Id)
    {
        var user = _userRepository.GetDocumentByTrn(Id);
        return user;
    }
    public static User UpdateUser(Message message)
    {
        var parent = _userRepository.GetDocument(ArgumentParser.Parse(message.Text).ArgumentsText);
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
        DeleteUser(GetUser(Id));
    }
    public static void DeleteUser(User user)
    {
        _userRepository.DeleteDocumentAsync(user);
    }
    
     public static List<User> ListChildrenFrom(string parent, DateTime dateTime)
    {
        return _userRepository.GetDocuments(parent, dateTime);
    }


    public static void CreateCategory(Message message)
    {
        var messageArgs = ArgumentParser.Parse(message.Text).ArgumentsText;
        CreateCategory(messageArgs);
    }
    public static void CreateCategory(string Title)
    {
        var category = new Category()
        {
            Title = Title
        };
        _categoryRepository.CreateDocument(category);   
    }
    public static async Task<Category> GetCategory(string Title)
    {
        var category = await _categoryRepository.GetDocumentAsync(Title);
        if (category == null) return null;
        return category;
    }
    public static async Task<Category> GetCategory(long telegramId)
    {
        var category = await _categoryRepository.GetDocumentAsync(telegramId);
        if (category == null) return null;
        return category;
    }
    public static async Task<Category> GetCategoryAsync(long Id)
    {
        return await _categoryRepository.GetDocumentAsync(Id);
    }
    public static async Task<Category> GetCategoryAsync(Guid Id)
    {
        return await _categoryRepository.GetDocumentAsync(Id);
    }
    public static async Task<Category> GetCategoryAsync(Message message)
    {
        return await GetCategory(ArgumentParser.Parse(message.Text).ArgumentsText);
    }
    public static async Task<List<Category>> GetAllCategories()
    {
        return await _categoryRepository.GetAllDocumentsAsync();
    }

    public static async Task CreateChannel(Channel channel)
    {
        if(channel.TelegramId == 0) channel.TelegramId = await ChannelInfo.LoginChat("@" + channel.Title);
        _channelRepository.CreateDocument(channel);
    }
    public static void CreateChannel(string Title)
    {
        var channel = new Channel()
        {
            TelegramId = new long(),
            Title = Title
        };
        CreateChannel(channel).Wait();
    }
    
    public static Channel GetChannel(Message message)
    {
        var channel = _channelRepository.GetDocument(ArgumentParser.Parse(message.Text).ArgumentsText);
        return channel;
    }
    public static Channel GetChannel(string name)
    {
        var channel = _channelRepository.GetDocument(name);
        return channel;
    }
    public static async Task<Channel> GetChannelAsync(long Id)
    {
        var channel = await _channelRepository.GetDocumentAsync(Id);
        // if (channel == null) 
        return channel;
    }

    public static async Task<List<Channel>> FindChannelToListAsync(long user)
    {   
        List<Channel> channels = await _channelRepository.GetOldestDocuments(user, 20);
        return channels;
    }

   
}