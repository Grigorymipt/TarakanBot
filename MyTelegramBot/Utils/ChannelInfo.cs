using System.Text;
using TL;
using TL.Methods;

namespace MyTelegramBot.Types;

public static class ChannelInfo
{
    public static void AddChannelToList(){}
    
    private static async Task<List<long>> ListAllChannelUsers(string channelName)//
    {
        using var client = new WTelegram.Client(Environment.GetEnvironmentVariable);
        await client.LoginUserIfNeeded();
        
        long channelId = 0;
        long accessHash = 0;
        var chats = await client.Messages_GetAllChats(); // chats = groups/channels (does not include users dialogs)
        Console.WriteLine("This user has joined the following:");
        foreach (var (id, chat) in chats.chats)
            switch (chat)
            {
                case Channel channel1 when channel1.IsChannel && channel1.username == channelName:
                    channelId = id;
                    accessHash = channel1.access_hash;
                    break;
            }
        List<long> channelUsers = new List<long>();
        InputChannelBase inputChannelBase = new InputChannel(channelId, accessHash);
        var list = await client.Channels_GetAllParticipants(inputChannelBase);
        foreach (var element in list.participants)
        {
            channelUsers.Add(element.UserId);
            Console.WriteLine(element.UserId);
        }
        return channelUsers;
    }
    public static async Task<bool> Subscribed(string channelName, long userId)
    {
        List<long> subscribers = await ChannelInfo.ListAllChannelUsers(channelName);
        var user = subscribers.Find(u => u == userId);
        if (user == userId)
            return true;
        return false;
    }
}