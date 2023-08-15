using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TL.Methods;

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
        
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "repost.jpg", cancellationToken);

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
            if (context.BotClient.IsAdmin("@" + newChannel, context.Update.Message.From.Id).Result) 
            {
                if(newUser.Channels == null || newUser.Channels.Contains(newChannel) == false)
                {
                    newUser.Channels ??= new List<string>();
                    newUser.Channels.Add(newChannel); // FIXME: very strange behavior
                    Channel channel = new Channel()
                    {
                        PersonID = user.TelegramId,
                        Title = newChannel,
                    };
                    Database.CreateChannel(channel);
                }
                else
                {
                    Buttons.Clear();
                    return "Вы уже добавляли данный канал";
                }
            }
            else
            {
                Buttons.Clear();
                return "Вы не являетесь создателем данного канала";
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "Channel not Exists")
            {
                Buttons.Clear();
                Buttons.Add("Попробовать еще раз", "/saveCategory");
                return "Такого канала не существует";
            }
            Log.Error(ex.ToString());
        }
        newUser.LastMessage = null;
        newUser.Update();
        return MessageToSend[0];
    }
}