using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class ContinueQuery : Query, IListener
{
    public ContinueQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/continueTo" };
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "subscriptions.jpg", cancellationToken);
        var result = CheckFiveCategories(context, out buttons);
        return result;
    }

    private string CheckFiveCategories(Context context, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>();
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        var MessageToSend = Globals.GetCommand(Globals.GetCommand("choosenmore")) + (5 - user.Categories.Count);
        if (user is { Categories.Count: >= 5 })
        {
            MessageToSend = Globals.GetCommand("nicetaste");
            if (user is { Categories.Count: >= 5 })
                buttons.Clear();
            buttons.Add(Globals.GetCommand("StartSubscribing"), "/subscribeTenChannels");
        }

        return MessageToSend;
    }
}