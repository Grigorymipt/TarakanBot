using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class CorrectTestAnswer : Query, IListener
{
    public CorrectTestAnswer(Bot bot) : base(bot)
    {
        Names = new[] { "/hundredMillions" };
        InitButtons();
        MessageToSend = "🤑 Верно! Еще несколько простых действий и миллион подписчиков твой! + Выбери минимум 5 тем," +
                        " которые тебя РЕАЛЬНО интересуют.";
    }
    private async void InitButtons()
    {
        var categories = await Database.GetAllCategories();
        Buttons = new Dictionary<string, string>();
        foreach (var category in categories)
        {
            Buttons.Add(category.Title, "/saveCategoryToUser " + category.Id);
        }
        Buttons.Add("Продолжить", "/continueTo");
    }
}
