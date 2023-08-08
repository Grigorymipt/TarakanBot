using System.Threading.Channels;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TL;
using Channel = TL.Channel;
using Message = Telegram.Bot.Types.Message;
using User = MongoDatabase.ModelTG.User;

namespace MyTelegramBot.Listeners;

public class SubscribeTenChannelsQuery : Query, IListener
{
    // protected List<string> ChannelNames = new List<string>() { "google.com" };

    // private List<MongoDatabase.ModelTG.Channel> channels;
    // protected List<MongoDatabase.ModelTG.Channel> Channels
    // { 
    //     get
    //     {
    //         channels ??= Database.FindChannelToListAsync().Result;
    //         return channels;
    //     }
    //     set 
    //     {
    //         channels = value;
    //     }
    // }
    // private string channelName;
    protected MongoDatabase.ModelTG.Channel ChannelName(long userId)
    {
        //FIFO logics
        var channel = Database.FindChannelToListAsync().Result.First();
        var user = Database.GetUser(userId);
        do
        {
            channel.dateTime = DateTime.Now;
            channel.Update();
        } while (user.Channels?.Contains(channel.Title) == true);    
        user.Subscribes??=new List<MongoDatabase.ModelTG.Channel>();
        user.Subscribes.Add(channel);
        user.Update();
        return channel;
    }


    public SubscribeTenChannelsQuery(Bot bot) : base(bot)
    {
        MessageToSend = "Some @ - channel with short description, EX: " + "Channel Name"; //uncomme
        Names = new[] { "/subscribeTenChannels" };
        //Links = ...
        Buttons = new Dictionary<string, string>()
        {
            { "🟢 Подписаться", "/subscribeListedChannel" }, // MakeLink
            { "🔴 Пропустить", "/skipListedChannel" },
            { "🔴 Black List 🔴", "/blockListedChannel " },
            { "Подписался на 10 каналов", "/iSubscribed" }
        };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user.Subscribes?.Count > 5) //TODO: 20 in prod
        {
            MessageToSend = "Вы слишком много раз нажали кнопку пропустить. Подпишитесь как минимум на десять" +
                            " каналов, предложенных выше.";
            Buttons.Clear(); //FIXME
        }
        // if (ChannelName == null) MessageToSend = "В #Userhub меньше 20 каналов, подпишитесь на представленные выше";
        
        return base.Run(context, cancellationToken);
    }

    public override async Task Handler(Context context, Dictionary<string, string> buttonsList, CancellationToken cancellationToken)
    {
        string response = await RunAsync(context, cancellationToken);
        Int64 chatId = context.Update.CallbackQuery.Message.Chat.Id;

        List<IEnumerable<InlineKeyboardButton>> categoryList = new List<IEnumerable<InlineKeyboardButton>>();
        var channeltosubs = ChannelName(context.Update.CallbackQuery.From.Id);
        MessageToSend = channeltosubs.Title + channeltosubs.Describtion; 
        foreach (var category in buttonsList)
        {
            InlineKeyboardButton reply;
            
            if (category.Value == "/subscribeListedChannel" && channeltosubs != null)
            { 
                try
                {
                    // Console.WriteLine("");
                    reply = InlineKeyboardButton
                        .WithUrl(category.Key, "https://t.me/" + channeltosubs.Title);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                 reply = InlineKeyboardButton
                    .WithCallbackData(category.Key, category.Value);
            }

            IEnumerable<InlineKeyboardButton> inlineKeyboardButton = new[] { reply };
            categoryList.Add(inlineKeyboardButton);
        }

        IEnumerable<IEnumerable<InlineKeyboardButton>> enumerableList1 = categoryList;
        InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(enumerableList1);

        Message sentMessage = await context.BotClient.SendTextMessageAsync(
            chatId: chatId,
            text: response,
            parseMode: Config.ParseMode,
            replyMarkup: inlineKeyboardMarkup
        );
    }
}

public class SkipTenChannelsQuery : SubscribeTenChannelsQuery
{
    public SkipTenChannelsQuery(Bot bot) : base(bot)
    {
        Names = new []{"/skipListedChannel"};
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        // User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        // if(user.Subscribes == null) user.Subscribes = new List<MongoDatabase.ModelTG.Channel>();
        // user.Subscribes.Add(Database.GetChannel(ChannelName(context.Update.CallbackQuery.From.Id)));
        // user.Update();
        return base.Run(context, cancellationToken);
    }
}

public class BlockTenChannelsQuery : SubscribeTenChannelsQuery
{
    public BlockTenChannelsQuery(Bot bot) : base(bot)
    {
        Names = new []{"/blockListedChannel"};
        MessageToSend = "🤯 Благодарим! 🧐 Наша полиция нравов обязательно разберется с этим! \n\n" + MessageToSend;
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var channel = Database.GetChannel(context.Update.CallbackQuery.Message);
        if (channel != null)
        {
            channel.Reports += 1;
            channel.Update();
        }
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user.Subscribes?.Count > 5) //TODO: 20 in prod
        {
            MessageToSend ="🤯 Благодарим! 🧐 Наша полиция нравов обязательно разберется с этим! \n\n" +
                           "Вы слишком много раз нажали кнопку пропустить. Подпишитесь как минимум на десять" +
                            " каналов, предложенных выше.";
            Buttons.Clear(); //FIXME
        }
        return MessageToSend;
    }
}

class CheckSubscriptions : SubscribeTenChannelsQuery, IListener
{
    private bool UserSubscribed = false;

    public CheckSubscriptions(Bot bot) : base(bot)
    {
        Names = new[] { "/iSubscribed" };
        Buttons = new Dictionary<string, string>();
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var userId = context.Update.CallbackQuery.From.Id;
        int totalAmount = 0;
        User user = Database.GetUser(userId);
        foreach (var channel in 
            user.Subscribes ??= new List<MongoDatabase.ModelTG.Channel>())
        {
            var userSubscribed = ChannelInfo.Subscribed(channelName: channel.Title, userId).Result;
            if (userSubscribed) UserSubscribed = true;
            if (UserSubscribed)
                totalAmount += 1;
        }
        if (totalAmount < 1) // TODO: prod - 10
        {
            MessageToSend = "вы не подписаны на n, каналов, не надо так(";
        }
        else
        {
            MessageToSend = "🎯 Отлично, теперь необходимо подписаться на 10 каналов VIP блоггеров. При нажатии на " +
                                "кнопку пропустить канал будет заменен на другой, исходя из указанных интересов. " +
                                "Нажать 'пропустить' можно не более 20 раз. 🚨🚔 Если канал нарушает правила пользования " +
                                "#UserHub, то жми «пропустить» а затем «Black List» и наши специалисты разберутся с этим.";
                Buttons.Clear();
                Buttons.Add("Принято!", "/subscribeTenVIPChannels");
        }

        return base.Run(context, cancellationToken);
    }
    // public override async Task Handler(Context context, Dictionary<string, string> buttonsList, CancellationToken cancellationToken)
    // {
    //     base.Handler(context, buttonsList, cancellationToken);
    // }
}
