using System.Threading.Channels;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TL;
using Channel = MongoDatabase.ModelTG.Channel;
using Message = Telegram.Bot.Types.Message;
using User = MongoDatabase.ModelTG.User;
using Serilog;

namespace MyTelegramBot.Listeners;

public class SubscribeTenChannelsQuery : Query, IListener
{
    protected MongoDatabase.ModelTG.Channel ChannelName(long userId)
    {
        //FIFO logics
        var channel = Database.FindChannelToListAsync(userId).Result.First();
        var user = Database.GetUser(userId);
        user.Subscribes ??= new List<MongoDatabase.ModelTG.Channel>();
        if (user.SubscribesVip.Contains(channel) == false) 
        { 
            channel.dateTime = DateTime.Now;
            channel.Update();
            user.Subscribes.Add(channel);
            user.Update();
            return channel;
        }
        else 
        {
            throw new Exception("ChannelsEnded");
        }
    }


    public SubscribeTenChannelsQuery(Bot bot) : base(bot)
    {
        MessageToSend = new string[]{ 
            "ShowChannel", 
            "Вы слишком много раз нажали кнопку пропустить. Подпишитесь как минимум на десять каналов, предложенных выше."
            };
        Names = new[] { "/subscribeTenChannels" };
        //Links = ...
        
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>()
        {
            { "🟢 Подписаться", "/subscribeListedChannel" }, // MakeLink
            { "🔴 Пропустить", "/skipListedChannel" },
            { "🔴 Black List 🔴", "/blockListedChannel " },
            { "Подписался на 10 каналов", "/iSubscribed" }
        };
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user?.Subscribes?.Count > 5) //TODO: 20 in prod
        {
            Buttons.Clear(); //FIXME
            return MessageToSend[1];   
        }
        return MessageToSend[0];
        // if (ChannelName == null) MessageToSend = "В #Userhub меньше 20 каналов, подпишитесь на представленные выше";
    }

    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        var buttons = new Dictionary<string, string>(){};
        string response = Run(context, cancellationToken, out buttons);
        Log.Information("run response received");
        Int64 chatId = context.Update.CallbackQuery.Message.Chat.Id;
        List<IEnumerable<InlineKeyboardButton>> categoryList = new List<IEnumerable<InlineKeyboardButton>>();
        Channel channeltosubs;
        try 
        {
            channeltosubs = ChannelName(context.Update.CallbackQuery.From.Id);
        }
        catch(Exception ex)
        {
            if(ex.Message == "ChannelsEnded") channeltosubs = new Channel();
        }
        channeltosubs = new Channel();
        response = response == MessageToSend[0] ? (channeltosubs.Title + channeltosubs.Describtion) : response;
        Log.Information("handling buttons");
 
        foreach (var category in buttons)
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
        Log.Information("start sending");
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
        MessageToSend = new string[] {
            "ShowChannel",
            "🤯 Благодарим! 🧐 Наша полиция нравов обязательно разберется с этим! \n\n" + "ShowChannel", 
            "🤯 Благодарим! 🧐 Наша полиция нравов обязательно разберется с этим! \n\n" +
                           "Вы слишком много раз нажали кнопку пропустить. Подпишитесь как минимум на десять" +
                            " каналов, предложенных выше."};
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "goodbuy.jpeg", cancellationToken);
        base.Run(context, cancellationToken, out Buttons);
        var channel = Database.GetChannel(context.Update.CallbackQuery.Message);
        if (channel != null)
        {
            channel.Reports += 1;
            channel.Update();
        }
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user.Subscribes?.Count > 5) //TODO: 20 in prod
        {
            Buttons.Clear(); //FIXME
            return MessageToSend[2];
        }
        return MessageToSend[1];
    }
}

class CheckSubscriptions : SubscribeTenChannelsQuery, IListener
{
    private bool UserSubscribed = false;

    public CheckSubscriptions(Bot bot) : base(bot)
    {
        Names = new[] { "/iSubscribed" };
        MessageToSend = base.MessageToSend
        .Append("вы не подписаны на n, каналов, не надо так(")
        .Append("🎯 Отлично, теперь необходимо подписаться на 10 каналов VIP блоггеров. При нажатии на " +
                                "кнопку пропустить канал будет заменен на другой, исходя из указанных интересов. " +
                                "Нажать 'пропустить' можно не более 20 раз. 🚨🚔 Если канал нарушает правила пользования " +
                                "#UserHub, то жми «пропустить» а затем «Black List» и наши специалисты разберутся с этим.").ToArray();
    
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>();
        var userId = context.Update.CallbackQuery.From.Id;
        int totalAmount = 0;
        User user = Database.GetUser(userId);
        foreach (var channel in 
            user.Subscribes ??= new List<MongoDatabase.ModelTG.Channel>())
        {
            Log.Information("userSubscribed:");
            var userSubscribed = context.BotClient.MemberStatusChat(channelName: channel.Title, userId).Result;
            Log.Information(userSubscribed);
            if (userSubscribed == "MemMemberber") UserSubscribed = true;
            if (UserSubscribed)
                totalAmount += 1;
        }
        if (totalAmount < 1) // TODO: prod - 10
        {
            return "вы не подписаны на 1 каналов, не надо так(";
        }
        else
        {
            Buttons.Clear();
            Buttons.Add("Принято!", "/clear66step"); //TODO: PROD: subscribeTenVIPChannels
            return MessageToSend.Last();
                
        }
    }
    // public override async Task Handler(Context context, Dictionary<string, string> buttonsList, CancellationToken cancellationToken)
    // {
    //     base.Handler(context, buttonsList, cancellationToken);
    // }
}
