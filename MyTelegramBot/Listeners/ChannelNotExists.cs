using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class ChannelNotExists : Query, IListener
{
    public ChannelNotExists(Bot bot) : base(bot)
    {
        Names = new[] { "/channelExists" };
        MessageToSend = new[] {Globals.responses.GetValueOrDefault("notexists")};
    }
}