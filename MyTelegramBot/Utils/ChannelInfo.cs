using System.Text;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using TL;
using TL.Methods;

namespace MyTelegramBot.Types;

[Obsolete]
public static class ChannelInfo
{
    // ITelegramBotClient botClient = Login();
    private static string Config(string what)
        {
            switch (what)
            {
                case "api_id": return Environment.GetEnvironmentVariable("api_id");
                case "api_hash": return Environment.GetEnvironmentVariable("api_hash");
                case "phone_number": return Environment.GetEnvironmentVariable("phone_number");
                case "verification_code": 
                    Console.WriteLine("You have 30 seconds to login. Please enter verification code.");
                    Log.Information("You have 30 seconds to login. Please enter verification code.");
                    Thread.Sleep(30*1000);
                    if (Environment.GetEnvironmentVariable("VerificationCode") == null) throw new ArgumentNullException("You haven`t send verification code.");
                    return Environment.GetEnvironmentVariable("VerificationCode");
                default: return null;
            }
        }
    public static async Task Login(ITelegramBotClient botClient)
    {
        using var client = new WTelegram.Client(Config);
        await client.LoginUserIfNeeded();
        botClient = botClient;
    }
    private static async Task<RegData> SaveChannelRegInfo(string channelName) // TODO: FIXME dont call client before close
    {
        using var client = new WTelegram.Client(Config);
        await client.LoginUserIfNeeded();

        var resolved = await client.Contacts_ResolveUsername(channelName);
        if (resolved.Chat is Channel channel)
        {
            await client.Channels_JoinChannel(channel);
            //Save to db to have acceshash:
            var RegData = GetChannels(channelName).Result;
            var channelDB = Database.GetChannel(RegData.ChannelTitle);
            if (channelDB != null)
            {
                channelDB.TelegramId = RegData.ChannelId;
                channelDB.AccessHash = RegData.AccessHash;
            } 
            return RegData;
        }
        Console.WriteLine($"{channelName} not a ChaCnel");
        Log.Information($"{channelName} not a ChaCnel"); 
        throw new NullReferenceException("Channel not Exists");
    }
    private record RegData(long ChannelId = 0, long AccessHash = 0, string ChannelTitle = "");
    private static async Task<RegData> GetChannels(string channelName)
    {
        using var client = new WTelegram.Client(Config);
        await client.LoginUserIfNeeded();
        
        var chats = await client.Messages_GetAllChats(); // chats = groups/channels (does not include users dialogs)
        Console.WriteLine("This user has joined the following:");
        Log.Information("This user has joined the following:");

        foreach (var (id, chat) in chats.chats)
            switch (chat)
            {
                case Channel channel1 when channel1.IsChannel && channel1.username == channelName:
                    return new RegData(id, channel1.access_hash, channel1.title);
            }
        return new RegData();
    }
    
    private static async Task<Channels_ChannelParticipants> ListAllChannelUsers(string channelName, ChannelParticipantsFilter filter = null)//
    {
        var RegData = await GetChannels(channelName);
        using var client = new WTelegram.Client(Config);
        await client.LoginUserIfNeeded();
        try
        {
            InputChannelBase inputChannelBase = new InputChannel(RegData.ChannelId, RegData.AccessHash);
            var list = await client.Channels_GetParticipants(inputChannelBase, filter: filter);
            return list;
        }
        catch
        {
            try
            {
                new CancellationTokenSource().CancelAfter(30);
                SaveChannelRegInfo(channelName).Wait();
                InputChannelBase inputChannelBase = new InputChannel(RegData.ChannelId, RegData.AccessHash);
                var list = await client.Channels_GetParticipants(inputChannelBase, filter: filter);
                return list;
            }
            catch(Exception ex)
            {
                // if(ex is TimeoutException) 
                throw;
            }
        }
        
    }
    [Obsolete]
    public static async Task<bool> Subscribed(string channelName, long userId)
    {
        var subscribers = await ListAllChannelUsers(channelName);
        List<long> channelUsers = new List<long>();
        foreach (var element in subscribers.participants)
        {
            channelUsers.Add(element.UserId);
            // Console.WriteLine(element.UserId);
        }
        var user = channelUsers.Find(u => u == userId);
        if (user == userId)
            return true;
        return false;
    }
    [Obsolete]
    public static async Task<bool> IsAdmin(string channelName, long userId)
    {
        try
        {
            var participants = await ListAllChannelUsers(channelName, new ChannelParticipantsAdmins());
            foreach (var participant in participants.participants)
            // This is the better way to enumerate the result
                if (participant is ChannelParticipantCreator cpc && cpc.user_id == userId) {
                    Console.WriteLine(cpc.user_id);
                    Log.Information(cpc.user_id.ToString());
                    return true;}
                
            return false;
        }
        catch(Exception ex)
        {
            throw;
        }
    }

    private static async Task<bool> IsAdmin(this ITelegramBotClient botClient, long channelId, long userId)
    {
        if (await MemberStatusChat(botClient, channelId, userId) == "Admin") return true;
        return false;
    }
    public static async Task<bool> IsAdmin(this ITelegramBotClient botClient, string channelName, long userId)
    {
        // var channelId = SaveChannelRegInfo(channelName).Result.ChannelId; //try to get rid of using Telegram API
        var message = await botClient.SendTextMessageAsync(
            chatId: channelName,
            text: $"auxiliary message from {botClient.GetMeAsync().Result}, this will be removed soon",
            disableNotification: true
        );
        long channelId = message.Chat.Id;
        await botClient.DeleteMessageAsync(channelId, message.MessageId);
        var channelDB = Database.GetChannel(channelName);
        if (channelDB != null)
        {
            channelDB.TelegramId = channelId;
        } 
        
        if (await MemberStatusChat(botClient, channelId, userId) == "Admin") return true;
        return false;
    }

    public static async Task<bool> CheckMessageAutor(string channelName, int postId, int repostId)
    {        
        var RegData = await GetChannels(channelName);
        using var client = new WTelegram.Client(Config);
        await client.LoginUserIfNeeded();
        InputChannelBase inputChannelBase = new InputChannel(RegData.ChannelId, RegData.AccessHash);


        var channels = await client.Channels_GetChannels(inputChannelBase);
        var channel = channels.chats.FirstOrDefault();
        var messages = await client.Channels_GetMessages(inputChannelBase, postId);  
        Console.WriteLine("----------------" + messages.Messages.Count());
        Log.Information("----------------" + messages.Messages.Count());
        
        foreach (var msgBase in messages.Messages)
        {  
            if (msgBase is TL.Message message)
            {
                Console.WriteLine(message.fwd_from.post_author);
                Log.Information(message.fwd_from.post_author);
                Console.WriteLine(message.fwd_from);
                Console.WriteLine(client.User.username);
                Log.Information(client.User.username);
                if (message.fwd_from.post_author == client.User.username) return true;
            }
            else
            {
                Console.WriteLine("Not a message: " + msgBase.ToString());
            }
        }
        return false;
    }
   

    public static async Task<string> MemberStatusChat(this ITelegramBotClient botClient, string channelName, long userId)
    {
        var channelId = Database.GetChannel(channelName).TelegramId;
        return await botClient.MemberStatusChat(channelId, userId);
    }

    public static async Task<string> MemberStatusChat(this ITelegramBotClient botClient, long channelId, long userId)
    {
        string status = "Nobody";
        try
        {
            ChatMember member = await botClient.GetChatMemberAsync(channelId, userId);
            status = member.Status.ToString();
            Log.Information($"User: {member.User.Username} is a {status}");
        }
        catch(Exception ex)
        {
            if (ex is ApiRequestException arex)
            Log.Error($"User: {userId} not found in {channelId}");
            status = "Nobody";
        }
        return status;
    }
}
