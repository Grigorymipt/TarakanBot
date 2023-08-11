using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using TL;
using TL.Methods;

namespace MyTelegramBot.Types;

public static class ChannelInfo
{
    public static void AddChannelToList(){}
    private static string Config(string what)
        {
            switch (what)
            {
                case "api_id": return Environment.GetEnvironmentVariable("api_id");
                case "api_hash": return Environment.GetEnvironmentVariable("api_hash");
                case "phone_number": return Environment.GetEnvironmentVariable("phone_number");
                case "verification_code": 
                    Console.WriteLine("You have 30 seconds to login. Please enter verification code.");
                    Thread.Sleep(30*1000);
                    if (Environment.GetEnvironmentVariable("VerificationCode") == null) throw new ArgumentNullException("You haven`t send verification code.");
                    return Environment.GetEnvironmentVariable("VerificationCode");
                default: return null;
            }
        }
    public static async Task Login()
    {
        using var client = new WTelegram.Client(Config);
        await client.LoginUserIfNeeded();
    }
    private static async Task SaveChannelRegInfo(string channelName) // TODO: FIXME dont call client before close
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
                channelDB.AccessHash = RegData.AccessHash;
            } 
        }
        Console.WriteLine($"{channelName} not a ChaCnel");
        throw new NullReferenceException("Channel not Exists");
    }
    private record RegData(long ChannelId = 0, long AccessHash = 0, string ChannelTitle = "");
    private static async Task<RegData> GetChannels(string channelName)
    {
        using var client = new WTelegram.Client(Config);
        await client.LoginUserIfNeeded();
        
        var chats = await client.Messages_GetAllChats(); // chats = groups/channels (does not include users dialogs)
        Console.WriteLine("This user has joined the following:");

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
    public static async Task<bool> IsAdmin(string channelName, long userId)
    {
        try
        {
            var participants = await ListAllChannelUsers(channelName, new ChannelParticipantsAdmins());
            foreach (var participant in participants.participants) // This is the better way to enumerate the result
                if (participant is ChannelParticipantCreator cpc && cpc.user_id == userId) {Console.WriteLine(cpc.user_id); return true;}
            return false;
        }
        catch(Exception ex)
        {
            throw;
        }
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
        foreach (var msgBase in messages.Messages)
        {  
            if (msgBase is TL.Message message)
            {
                Console.WriteLine(message.fwd_from);
                Console.WriteLine(client.User.username);
                if (message.fwd_from.post_author == client.User.username) return true;
            }
            else
            {
                Console.WriteLine("Not a message: " + msgBase.ToString());
            }
        }
        return false;
    }
}