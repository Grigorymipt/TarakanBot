using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Telegram.Bot.Types;
using InlineQuery = MyTelegramBot.Types.InlineQuery;

namespace MyTelegramBot.Listeners;

public class AlmostOnTarget2Query : InlineQuery, IListener
{
    public AlmostOnTarget2Query(Bot bot) : base(bot)
    {
        Names = new[] { "/clear66step" };
        MessageToSend = "🤙 Отлично! 🎯 Мы уже у цели! Отправь мне Username канала, который будем продвигать, в " +
                        "таком виде: @channel💡 Канал обязательно должен быть открытым.";
        MessageLabel = "GetAddressInline2";
    }

    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        base.Handler(context, cancellationToken);
        MessageToSend = "Отправь мне Username канала, который будем продвигать, в " +
        "таком виде: @channel💡 Канал обязательно должен быть открытым.";
    }
}
public class CheckChannelExistence : InlineReply, IListener
{
    public CheckChannelExistence(Bot bot) : base(bot)
    {
        MessageLabel = "GetAddressInline2";
        Buttons = new Dictionary<string, string>();
    }
    private bool ChannelExists(Context context, CancellationToken cancellationToken = default)
    {
        // TODO some logic:  check owner rights and add to DB if owner
        return true;
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        if (ChannelExists(context, cancellationToken))
        {
            MessageToSend = "🎯 Канал успешно добавлен! 🚨 ВАЖНО 🚨 Если удалить бота @ТАКОГО из администраторов, то" +
                            " ваш канал выпадет из выдачи и вы перестанете получать подписчиков. 🤫 С годовой " +
                            "VIP-подпиской можно добавить сразу 2 канала.";
            Buttons.Clear();
            Buttons.Add("🤠 Продолжить бесплатно", "/continueFree");
            Buttons.Add("🏆 Подробнее о статусе VIP", "/moreAboutVIP");
        }
        else
        {
            MessageToSend =
                "🤯 Такого канала в твое владении не существует. Проверь, пожалуйста, Username и отправь сообщение " +
                "еще раз!";
            Buttons.Clear();
            Buttons.Add("Отправить еще раз", "/clear66step");
        }
        var user = Database.GetUser(context.Update.Message.From.Id);
        string newChannel = context.Update.Message.Text;
        var newUser = user;
        newUser.Channels.Add(newChannel); // FIXME: very strange behavior
        Channel channel = new Channel()
        {
            PersonID = user.Id,
            Title = newChannel,
        }; // TODO: remove creating channel in abstract class
        Database.CreateChannel(channel);
        newUser.LastMessage = null;
        newUser.Update();
        return MessageToSend;
    }
}

public class ContinueFreeQuery : Query, IListener
{
    public ContinueFreeQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/continueFree" };
        MessageToSend = "🥳 Поздравляю! Ты сделал это! До 1.000.000 подписчиков остался всего 1 шаг. \n" +
                        "Твоя персональная ссылка на сервис #UserHub \n------------------ссылка------------------- \n." +
                        "Рассказывай о сервисе #UserHub и все приглашенные на 10-и уровнях рукопожатий подпишутся " +
                        "на твой канал. В разделе /Menu можно найти рекламные промо материалы! 🚀 Вперед к своему " +
                        "первому 1.000.000 подписчиков! Ссылка на чат комьюнити блоггеров! \n" +
                        "------------------ссылка-------------------";
    }
}