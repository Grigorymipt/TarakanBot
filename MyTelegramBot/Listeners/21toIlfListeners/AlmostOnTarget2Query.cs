using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Telegram.Bot.Types;
using InlineQuery = MyTelegramBot.Types.InlineQuery;

namespace MyTelegramBot.Listeners;

public class AlmostOnTarget2Query : InlineQuery, IListener
{
    public AlmostOnTarget2Query(Bot bot) : base(bot)
    {
        Names = new[] { "/clear66step" };
        MessageToSend = new[] { "" };
        MessageLabel = "GetAddressInline2";
    }
}

public class CheckChannelExistence : InlineReply, IListener
{
    public CheckChannelExistence(Bot bot) : base(bot)
    {
        MessageLabel = "GetAddressInline2";
    }
    private bool ChannelExists(Context context, CancellationToken cancellationToken = default)
    {
        var channelName = context.Update.Message.Text;
        var userId = context.Update.Message.From.Id;
        return ChannelInfo.IsAdmin(channelName, userId).Result;
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>();
        string MessageToSend;
        if (ChannelExists(context, cancellationToken))
        {
            MessageToSend = Globals.GetCommand("sendchannelname");
            Buttons.Clear();
            Buttons.Add(Globals.GetCommand("TryFree"), "/continueFree");
            Buttons.Add(Globals.GetCommand("MoreAboutVip"), "/moreAboutVIP");
        }
        else
        {
            MessageToSend = Globals.GetCommand("notexists2");
            Buttons.Clear();
            Buttons.Add(Globals.GetCommand("trysendagain"), "/clear66step");
        }
        var user = Database.GetUser(context.Update.Message.From.Id);
        string newChannel = context.Update.Message.Text;
        var newUser = user;
        newUser.Channels.Add(newChannel); // FIXME: very strange behavior
        Channel channel = new Channel()
        {
            Owner = user.TelegramId,
            Title = newChannel,
        }; // TODO: remove creating channel in abstract class
        Database.CreateChannel(channel);
        newUser.LastMessage = null;
        newUser.Update();
        return MessageToSend;
    }
}

public class ContinueFreeQuery : Query, IListener
{
    public ContinueFreeQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/continueFree" };
        MessageToSend = new[] {
            Globals.GetCommand("Congratulations")
                        };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "cat.jpg", cancellationToken);
        return MessageToSend[0];
    }
}