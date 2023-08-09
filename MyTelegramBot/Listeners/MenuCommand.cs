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
            { "🤝 Мои рукопожатия", "/myHandshakes" },
            { "Мои каналы", "/myChannels" },
            { "🏆 Vip статус", "/myVipStatus" },
            { "Каталог каналов", "/catalog" },
            { "Комьюнити #USERHUB", "/UserhubCommunity" },
            { "Промо материалы", "/promo" },
            { "Баланс", "/myBalance" },
        };
        return MessageToSend[0];
    }
}