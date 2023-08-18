using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Telegram.Bot.Types;
using InlineQuery = MyTelegramBot.Types.InlineQuery;
using Serilog;
namespace MyTelegramBot.Listeners;

public class AlmostOnTarget2Query : Query, IListener
{
    public AlmostOnTarget2Query(Bot bot) : base(bot)
    {
        Names = new[] { "/clear66step" };
    }
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters parameters = new();
        parameters.MessageToSend = Globals.GetCommand("sendchannelname");
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user == null) throw new NullReferenceException("user not found");
        if (user.Channels.Count == 0) throw new NullReferenceException("user does not contain any channeles");
        foreach (var channel in user.Channels)
            parameters.buttons.Add(channel, $"/selectChannelForListing {channel}");
        return parameters;
    }
}

public class CheckChannelExistence : Query, IListener
{
    public CheckChannelExistence(Bot bot) : base(bot)
    {
        Names = new[] { "/selectChannelForListing" };
    }
    private bool ChannelExists(Context context, CancellationToken cancellationToken = default)
    {
        var channelName = ArgumentParser.Parse(
            context.Update.CallbackQuery.Data).ArgumentsText;
        Log.Information("CheckChannelExistence: " + channelName);
        var userId = context.Update.CallbackQuery.From.Id;

        return context.BotClient.IsAdmin(channelName, userId).Result;
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>();
        string MessageToSend;
        if (ChannelExists(context, cancellationToken))
        {
            MessageToSend = Globals.GetCommand("SendChannelForListing");
            Buttons.Clear();
            Buttons.Add(Globals.GetCommand("TryFree"), "/continueFree");
            Buttons.Add(Globals.GetCommand("MoreAboutVip"), "/moreAboutVIP");
        }
        else
        {
            MessageToSend = Globals.GetCommand("NotExists2");
            Buttons.Clear();
            Buttons.Add(Globals.GetCommand("trysendagain"), "/clear66step");
        }
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        string newChannel = context.Update.CallbackQuery.Text;
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