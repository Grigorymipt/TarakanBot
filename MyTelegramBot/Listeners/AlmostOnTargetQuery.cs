using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Listeners;

public class AlmostOnTargetQuery : Command//TODO: must be query from previous step
{
    public AlmostOnTargetQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/almostOnTarget" };
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
    }

    
    public override string Run(Context context, CancellationToken cancellationToken)
    {
        Console.WriteLine(context.Update.Message.From.Id);
        var user = GetUserSync(context.Update.Message.From.Id);
        Console.WriteLine(context.Update.Message.Text);
        string newChannel = context.Update.Message.Text;
        var newUser = user;
        newUser.Channels.Add(newChannel); // FIXME: very strange behavior
        newUser.LastMessage = null;
        newUser.Update();
        return MessageToSend;
    }

    public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.Message)
            return false;
        var user = GetUserSync(context.Update.Message.From.Id);
        if (user == null) return false;
        if (user.RefId == null)
            return false;
        if (user.LastMessage == "GetAddressInline")
            return true;
        return false;
    }
}