using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class ContinueQuery : Query, IListener
{
    public ContinueQuery(Bot bot) : base(bot)
    {
        Buttons = new Dictionary<string, string>();
        Names = new[] { "/continueTo" };
    }

    private void CheckFiveCategories(Context context)
    {
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        MessageToSend = "🤷 Необходимо выбрать еще " + (5 - user.Categories.Count) + " категорий и нажать «продолжить»!";
        if (user is { Categories.Count: >= 5 })
        {
            MessageToSend = "😍 Хороший вкус! Теперь необходимо подписаться на 10 каналов! Это те самые 10 каналов, " +
                            "которые используют систему #10рукопожатий и получают подписчиков БЕСПЛАТНО! При нажатии на " +
                            "кнопку 'пропустить' канал будет заменен на другой, в соответствии указанными интересами." +
                            " Не более 20 раз можно нажать «пропустить». 🚨🚔 Если канал нарушает правила пользования " +
                            "(кликабельно) #UserHub, то жми «пропустить», а затем «Black List» и наши специалисты разберутся с этим.";
            if(user is {Categories.Count: >= 5})
                Buttons.Clear();
                Buttons.Add("Начать подписываться", "/subscribeTenChannels");
        }
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "subscriptions.jpg", cancellationToken);
        CheckFiveCategories(context);
        return base.Run(context, cancellationToken);
    }

}