using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Listeners;

public class AlmostOnTargetQuery : InlineReply, IListener 
{
    public AlmostOnTargetQuery(Bot bot) : base(bot)
    {
        MessageToSend = new[] {"🎯 Мы почти у цели. Остался последний шаг!🤫 У меня к тебе деловое предложение. " +
                        "Предлагаю опубликовать в твоем канале репост одного из моих постов, где я рекомендую " +
                        "людям пользоваться каталогом #UserHub, а взамен я размещу твой канал в каталоге абсолютно" +
                        " бесплатно и навсегда. Если же этот вариант не подходит, то ты можешь приобрести пожизненный " +
                        "листинг в каталоге всего за 100$"};
        MessageLabel = "GetAddressInline";
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>()
        {
            {"🤝 Предложение принято", "/suggestionAccepted"},
            {"💳 Плачу за листинг", "/buyListingNow"}
        };
        // Console.WriteLine(context.Update.Message.From.Id);
        var user = Database.GetUser(context.Update.Message.From.Id);
        // Console.WriteLine(context.Update.Message.Text);
        string newChannel = context.Update.Message.Text;
        if (newChannel.First() != '@')
        {
            Buttons.Clear();
            Buttons.Add("Ввести имя канала заново","/saveCategory");
            return "Некорректное имя канала!";
        }
        var newUser = user;
        newChannel = newChannel.Remove(0, 1);
        try
        {
            if (ChannelInfo.IsAdmin(newChannel, context.Update.Message.From.Id).Result) 
            {
                if(newUser.Channels == null || newUser.Channels.Contains(newChannel) == false)
                {
                    newUser.Channels ??= new List<string>();
                    newUser.Channels.Add(newChannel); // FIXME: very strange behavior
                    Channel channel = new Channel()
                    {
                        PersonID = user.Id,
                        Title = newChannel,
                    };
                    Database.CreateChannel(channel);
                }
                else
                {
                    return "Вы уже добавляли данный канал";
                    Buttons.Clear();
                }
            }
            else
            {
                return "Вы не являетесь создателем данного канала";
                Buttons.Clear();
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "Channel not Exists")
            {
                return "Такого канала не существует";
                Buttons.Clear();
                Buttons.Add("Попробовать еще раз", "/saveCategory");
            }
        }
        newUser.LastMessage = null;
        newUser.Update();
        return MessageToSend[0];
    }
}