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
        MessageToSend = new[] { "" };
        MessageLabel = "GetAddressInline2";
    }
}

public class CheckChannelExistence : InlineReply, IListener
{
    public CheckChannelExistence(Bot bot) : base(bot)
    {
        MessageLabel = "GetAddressInline2";
    }
    private bool ChannelExists(Context context, CancellationToken cancellationToken = default)
    {
        // TODO some logic:  check owner rights and add to DB if owner
        return true;
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>();
        string MessageToSend;
        if (ChannelExists(context, cancellationToken))
        {
            MessageToSend = Globals.responses.GetValueOrDefault("AddedSuccessfully");
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
            Owner = user.TelegramId,
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
        MessageToSend = new[] {
                        "🥳 Поздравляю! Ты сделал это! До 1.000.000 подписчиков остался всего 1 шаг. \n" +
                        "Твоя персональная ссылка на сервис #UserHub \n------------------ссылка------------------- \n." +
                        "Рассказывай о сервисе #UserHub и все приглашенные на 10-и уровнях рукопожатий подпишутся " +
                        "на твой канал. В разделе /Menu можно найти рекламные промо материалы! 🚀 Вперед к своему " +
                        "первому 1.000.000 подписчиков! Ссылка на чат комьюнити блоггеров! \n" +
                        "------------------ссылка-------------------"
                        };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "cat.jpg", cancellationToken);
        return MessageToSend[0];
    }
}