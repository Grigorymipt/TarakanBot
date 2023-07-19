using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Listeners;

public class AlmostOnTargetQuery : InlineReply, IListener //TODO: must be query from previous step
{
    public AlmostOnTargetQuery(Bot bot) : base(bot)
    {
        Buttons = new Dictionary<string, string>()
        {
            {"🤝 Предложение принято", "/suggestionAccepted"},
            {"💳 Плачу за листинг", "/payForListing"}
        };
        MessageToSend = "🎯 Мы почти у цели. Остался последний шаг!🤫 У меня к тебе деловое предложение. " +
                        "Предлагаю опубликовать в твоем канале репост одного из моих постов, где я рекомендую " +
                        "людям пользоваться каталогом #UserHub, а взамен я размещу твой канал в каталоге абсолютно" +
                        " бесплатно и навсегда. Если же этот вариант не подходит, то ты можешь приобрести пожизненный " +
                        "листинг в каталоге всего за 100$";
        MessageLabel = "GetAddressInline";
    }
}