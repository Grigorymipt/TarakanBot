using MyTelegramBot.Types;
using Telegram.Bot.Types;
using InlineQuery = MyTelegramBot.Types.InlineQuery;

namespace MyTelegramBot.Listeners;

public class AlmostOnTarget2Query : InlineQuery
{
    public AlmostOnTarget2Query(Bot bot) : base(bot)
    {
        Names = new[] { "/clear66step" };
        MessageToSend = "🤙 Отлично! 🎯 Мы уже у цели! Отправь мне Username канала, который будем продвигать, в " +
                        "таком виде: @channel💡 Канал обязательно должен быть открытым.";
        MessageLabel = "GetAddressInline2";
    }
}
public class CheckChannelExistence : InlineReply
{
    public CheckChannelExistence(Bot bot) : base(bot)
    {
        MessageLabel = "GetAddressInline2";
        
    }
    private bool ChannelExists()
    {
        //some logic  
        return false;
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        if (ChannelExists())
        {
            MessageToSend = "🎯 Канал успешно добавлен! 🚨 ВАЖНО 🚨 Если удалить бота @ТАКОГО из администраторов, то" +
                            " ваш канал выпадет из выдачи и вы перестанете получать подписчиков. 🤫 С годовой " +
                            "VIP-подпиской можно добавить сразу 2 канала.";
            Buttons = new Dictionary<string, string>()
            {
                {"🤠 Продолжить бесплатно", "/continueFree"},
                {"🏆 Подробнее о статусе VIP", "/moreAboutVIP"}
            };
        }
        else
        {
            MessageToSend =
                "🤯 Такого канала в твое владении не существует. Проверь, пожалуйста, Username и отправь сообщение " +
                "еще раз!";
        }
        return base.Run(context, cancellationToken);
    }
}