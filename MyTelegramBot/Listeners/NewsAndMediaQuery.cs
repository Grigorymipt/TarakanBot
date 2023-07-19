using MyTelegramBot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Listeners;

public class NewsAndMediaQuery : Query, IListener
{
    public NewsAndMediaQuery(Bot bot): base(bot) {
        Names = new string[]{"/NewsAndMedia", "!news"};
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return "List of subcategories";
    }
}