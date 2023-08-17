using System.Collections;
using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Listeners;

public class CatalogCommand : Query, IListener
{
    public CatalogCommand(Bot bot): base(bot) {
        Names = new string[]{"/catalog"};
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        var categories = Database.GetAllCategories().Result;
        Buttons = new Dictionary<string, string>();
        foreach (var category in categories)
        {
            Buttons.Add(category.Title, "/smth");
        }
        return Globals.GetCommand("catalog");
    }
}


