using System.Text;
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
    private static async Task<Channels_ChannelParticipants> ListAllChannelUsers(string channelName, ChannelParticipantsFilter filter = null)//
    {
        
        using var client = new WTelegram.Client(Config);
        await client.LoginUserIfNeeded();
        
        long channelId = 0;
        long accessHash = 0;
        var chats = await client.Messages_GetAllChats(); // chats = groups/channels (does not include users dialogs)
        Console.WriteLine("This user has joined the following:");
        void InitAccessHash()
        {
            foreach (var (id, chat) in chats.chats)
                switch (chat)
                {
                    case Channel channel1 when channel1.IsChannel && channel1.username == channelName:
                        channelId = id;
                        accessHash = channel1.access_hash;
                        break;
                }
        }
        try
        {
            InitAccessHash();
            InputChannelBase inputChannelBase = new InputChannel(channelId, accessHash);
            var list = await client.Channels_GetParticipants(inputChannelBase, filter: filter);
            return list;
        }
        catch
        {
            try
            {
                var resolved = await client.Contacts_ResolveUsername(channelName); // without the @
                Console.WriteLine("resolved");
                if (resolved.Chat is Channel channel)
                {
                    await client.Channels_JoinChannel(channel);
                    Console.WriteLine($"Joined: {channel.Title}");
                    InitAccessHash();
                    InputChannelBase inputChannelBase = new InputChannel(channelId, accessHash);
                    new CancellationTokenSource().CancelAfter(30);
                    var list = await client.Channels_GetParticipants(inputChannelBase, filter: filter);
                    Console.WriteLine("LiSt");
                    return list;
                }
                Console.WriteLine($"{channelName} not a ChaCnel");
                throw new NullReferenceException("Channel not Exists");
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
}