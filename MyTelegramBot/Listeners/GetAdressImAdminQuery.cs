using System.Threading;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = MongoDatabase.ModelTG.User;

namespace MyTelegramBot.Listeners;

public class GetAdressImAdminQuery : Query, IListener
{
    public GetAdressImAdminQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/admin" };
        MessageToSend = new string[] {Globals.GetCommand("AddChannel"),};
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "admin.jpg", cancellationToken);
        buttons = new Dictionary<string, string>(){{Globals.GetCommand("AddChannelBut"), "/addChannel"}};
        return MessageToSend[0];
    }
}