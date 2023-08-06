using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Listeners;

public class AlmostOnTargetQuery : InlineReply, IListener 
{
    public AlmostOnTargetQuery(Bot bot) : base(bot)
    {
        Buttons = new Dictionary<string, string>()
        {
            {"🤝 Предложение принято", "/suggestionAccepted"},
            {"💳 Плачу за листинг", "/buyListingNow"}
        };
        MessageToSend = "🎯 Мы почти у цели. Остался последний шаг!🤫 У меня к тебе деловое предложение. " +
                        "Предлагаю опубликовать в твоем канале репост одного из моих постов, где я рекомендую " +
                        "людям пользоваться каталогом #UserHub, а взамен я размещу твой канал в каталоге абсолютно" +
                        " бесплатно и навсегда. Если же этот вариант не подходит, то ты можешь приобрести пожизненный " +
                        "листинг в каталоге всего за 100$";
        MessageLabel = "GetAddressInline";
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        // Console.WriteLine(context.Update.Message.From.Id);
        var user = Database.GetUser(context.Update.Message.From.Id);
        // Console.WriteLine(context.Update.Message.Text);
        string newChannel = context.Update.Message.Text;
        if (newChannel.First() == '@')
        {
            Buttons.Clear();
            Buttons.Add("Ввести имя канала заново","/saveCategory");
            return "Некорректное имя канала!";
        }
        var newUser = user;
        newUser.Channels.Add(newChannel); // FIXME: very strange behavior
        Channel channel = new Channel()
        {
            PersonID = user.Id,
            Title = newChannel,
        };
        Database.CreateChannel(channel);
        newUser.LastMessage = null;
        newUser.Update();
        return MessageToSend;
    }
}