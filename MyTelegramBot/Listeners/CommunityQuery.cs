using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class CommunityQuery : Query, IListener
{
    public CommunityQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/myHandshakes" };
        MessageToSend = new string[]{"Комьюнити"
        };
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            {"🔍 Все рукопожатия", "/allHandshakes"},
            {"1-й уровень", "/firstLevelHandshakes"},
        };
        return MessageToSend[0];
    }
}
