using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class CorrectTestAnswer : Query, IListener
{
    public CorrectTestAnswer(Bot bot) : base(bot)
    {
        Names = new[] { "/hundredMillions" };
        MessageToSend = new[] {"🤑 Верно! Еще несколько простых действий и миллион подписчиков твой! + Выбери минимум 5 тем," +
                        " которые тебя РЕАЛЬНО интересуют."};

                        
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        var categories = Database.GetAllCategories().Result;
        buttons = new Dictionary<string, string>();
        foreach (var category in categories)
        {
            buttons.Add(category.Title, "/saveCategoryToUser " + category.Title);
        }
        buttons.Add("Продолжить", "/continueTo");
        return MessageToSend[0];
    }
}
