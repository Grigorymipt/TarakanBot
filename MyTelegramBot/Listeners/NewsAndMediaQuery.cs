using MyTelegramBot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Listeners;

public class NewsAndMediaQuery : Query
{
    public NewsAndMediaQuery(Bot bot): base(bot) {
        Names = new string[]{"/NewsAndMedia", "!news"};
    }
    public override string Run(Context context, CancellationToken cancellationToken)
    {
        return "List of subcategories";
    }
}