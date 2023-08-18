using Amazon.Runtime.Internal.Transform;
using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class MyChannelsQuery : Query, IListener
{
    public MyChannelsQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/myChannels" };
        MessageToSend = new string[] {Globals.GetCommand("MyChannels")};
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>();
        foreach (var variableChannel 
                 in Database.GetUser(context.Update.CallbackQuery.From.Id).Channels)
        {
            buttons.Add(variableChannel, "/chooseChannel " + variableChannel);
        }
        return MessageToSend[0];
    }
}

public class ChooseChannelQuery : Query, IListener
{
    public ChooseChannelQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/chooseChannel" };
        MessageToSend = new string[] { Globals.GetCommand("SelectedChannel")
        };
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>()
        {
            { Globals.GetCommand("ChangeChannel"), "/changeChannel" },
            { Globals.GetCommand("ChannelStats"), "/handshakeStats"}, // TODO: not handshake
        };
        var channelName = ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText;
        Buttons.Add(Globals.GetCommand("VipStatus"), "/getVipStatusForChannel " + channelName);
        return MessageToSend + channelName;
    }
}

public class ChangeChannelQuery : Query, IListener
{
    public ChangeChannelQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/changeChannel" };
        MessageToSend = new string[] {Globals.GetCommand("CallSupport")};
    }
}