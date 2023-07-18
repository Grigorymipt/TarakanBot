using System.Collections;
using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Listeners;

public class CatalogCommand : Query
{
    public CatalogCommand(Bot bot): base(bot) {
        Names = new string[]{"/catalog"};
        InitButtons();
    }

    private async void InitButtons()
    {
        var categories = await Database.GetAllCategories();
        Buttons = new Dictionary<string, string>();
        foreach (var category in categories)
        {
            Buttons.Add(category.Title, "/smth");
        }
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return "Каталог категорий";
    }
}


