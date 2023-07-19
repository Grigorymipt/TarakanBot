using MyTelegramBot.Types;
using Telegram.Bot.Types;

namespace MyTelegramBot;
public class ChoseCategoryQuery : Query, IListener
{
    public ChoseCategoryQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/addChannel" };
        InitButtons();
        MessageToSend = "Выбери одну из категорий которая максимально подходит твоему каналу. ✅ ";
    }
    private async void InitButtons()
    {
        var categories = await Database.GetAllCategories();
        Buttons = new Dictionary<string, string>();
        foreach (var category in categories)
        {
            Buttons.Add(category.Title, "/saveCategory");
        }
    }
}