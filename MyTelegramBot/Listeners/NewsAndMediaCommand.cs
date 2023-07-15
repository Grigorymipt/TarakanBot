using MyTelegramBot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Listeners;

public class NewsAndMediaCommand : Query
{
    public NewsAndMediaCommand(Bot bot): base(bot) {
        Names = new string[]{"/NewsAndMedia"};
    }
    public override string Run(Context context, CancellationToken cancellationToken)
    {
        return "List of subcategories";
    }
}