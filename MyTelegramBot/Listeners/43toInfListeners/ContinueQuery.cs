using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class ContinueQuery : Query
{
    public ContinueQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/continue" };
    }
    public override string Run(Context context, CancellationToken cancellationToken)
    {
        CheckFiveCategories(context);
        return base.Run(context, cancellationToken);
    }

    private void CheckFiveCategories(Context context)
    {
        var user = GetUserSync(context.Update.CallbackQuery.From.Id);
        if (user is { Categories.Count: >= 5 })
        {
            MessageToSend = "😍 Хороший вкус! Теперь необходимо подписаться на 10 каналов! Это те самые 10 каналов, " +
                            "которые используют систему #10рукопожатий и получают подписчиков БЕСПЛАТНО! При нажатии на " +
                            "кнопку 'пропустить' канал будет заменен на другой, в соответствии указанными интересами." +
                            " Не более 20 раз можно нажать «пропустить». 🚨🚔 Если канал нарушает правила пользования " +
                            "(кликабельно) #UserHub, то жми «пропустить», а затем «Black List» и наши специалисты разберутся с этим.";
            Buttons = new Dictionary<string, string>(){{"🟢 Подписаться", "/subscribeTenChannels"}};
        }
        MessageToSend = "🤷 Необходимо выбрать еще .... категорий и нажать «продолжить»!";
    }
}