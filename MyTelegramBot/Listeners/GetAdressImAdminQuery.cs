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
        MessageToSend = new string[] {Globals.responses.GetValueOrDefault("addchannel"),};
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "admin.jpg", cancellationToken);
        buttons = new Dictionary<string, string>(){{Globals.responses.GetValueOrDefault("addchannelbut"), "/addChannel"}};
        return MessageToSend[0];
    }
}