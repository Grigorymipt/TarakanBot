using MyTelegramBot.Types;
using Telegram.Bot.Types;

namespace MyTelegramBot;
public class ChoseCategoryQuery : Query, IListener
{
    public ChoseCategoryQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/addChannel" };
        MessageToSend = new string[] { Globals.GetCommand("ChooseCategory")};
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        var categories = Database.GetAllCategories().Result;
        Buttons = new Dictionary<string, string>();
        foreach (var category in categories)
        {
            Buttons.Add(category.Title, "/saveCategory");
        }
        return MessageToSend[0];
    }
}