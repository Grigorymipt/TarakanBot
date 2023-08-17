using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class MenuCommand : Command, IListener
{
    public MenuCommand(Bot bot) : base(bot)
    {
        Names = new[] { "/menu" }; // TODO: Update validator
        MessageToSend = new string[]{"Меню:"};
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            { Globals.GetCommand("MyHandshakes"), "/myHandshakes" },
            { Globals.GetCommand("MyChannels"), "/myChannels" },
            { Globals.GetCommand("VipStatus"), "/myVipStatus" },
            { Globals.GetCommand("catalog"), "/catalog" },
            { Globals.GetCommand("Community"), "/UserhubCommunity" },
            { Globals.GetCommand("Promo"), "/promo" },
            { Globals.GetCommand("MyBalance"), "/myBalance" },
        };
        return MessageToSend[0];
    }
}