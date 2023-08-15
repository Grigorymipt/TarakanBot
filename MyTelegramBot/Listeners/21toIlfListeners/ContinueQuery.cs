using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class ContinueQuery : Query, IListener
{
    public ContinueQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/continueTo" };
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "subscriptions.jpg", cancellationToken);
        var result = CheckFiveCategories(context, out buttons);
        return result;
    }

    private string CheckFiveCategories(Context context, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>();
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        var MessageToSend = "🤷 Необходимо выбрать еще " + (5 - user.Categories.Count) + " категорий и нажать «продолжить»!";
        if (user is { Categories.Count: >= 5 })
        {
            MessageToSend = "😍 Хороший вкус! Теперь необходимо подписаться на 10 каналов! Это те самые 10 каналов, " +
                            "которые используют систему #10рукопожатий и получают подписчиков БЕСПЛАТНО! При нажатии на " +
                            "кнопку 'пропустить' канал будет заменен на другой, в соответствии указанными интересами." +
                            " Не более 20 раз можно нажать «пропустить». 🚨🚔 Если канал нарушает правила пользования " +
                            "(кликабельно) #UserHub, то жми «пропустить», а затем «Black List» и наши специалисты разберутся с этим.";
            if (user is { Categories.Count: >= 5 })
                buttons.Clear();
            buttons.Add("Начать подписываться", "/subscribeTenChannels");
        }

        return MessageToSend;
    }
}