using System.Threading.Channels;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TL;
using Channel = TL.Channel;
using Message = Telegram.Bot.Types.Message;
using User = MongoDatabase.ModelTG.User;

namespace MyTelegramBot.Listeners;

public class SubscribeTenChannelsVipQuery : Query, IListener
{
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
         
        user.SubscribesVip??=new List<MongoDatabase.ModelTG.Channel>();
        user.SubscribesVip.Add(channel);
        user.Update();
        return channel;
    }


    public SubscribeTenChannelsVipQuery(Bot bot) : base(bot)
    {
        MessageToSend = new string[]{ 
            "ShowChannel", 
            "Вы слишком много раз нажали кнопку пропустить. Подпишитесь как минимум на десять каналов, предложенных выше."
            };
        Names = new[] { "/subscribeTenVIPChannels" };
        //Links = ...
        
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>()
        {
            { "🟢 Подписаться", "/subscribeListedChannelVip" }, // MakeLink
            { "🔴 Пропустить", "/skipListedChannelVip" },
            { "🔴 Black List 🔴", "/blockListedChannelVip " },
            { "Подписался на 10 каналов", "/iSubscribedVip" }
        };
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user.SubscribesVip?.Count > 5) //TODO: 20 in prod
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
        string response = Task.Run(() => Run(context, cancellationToken, out buttons)).Result;
        Int64 chatId = context.Update.CallbackQuery.Message.Chat.Id;
        List<IEnumerable<InlineKeyboardButton>> categoryList = new List<IEnumerable<InlineKeyboardButton>>();
        var channeltosubs = ChannelName(context.Update.CallbackQuery.From.Id);
        response = response == MessageToSend[0] ? (channeltosubs.Title + channeltosubs.Describtion) : response; 
        foreach (var category in buttons)
        {
            InlineKeyboardButton reply;
            
            if (category.Value == "/subscribeListedChannelVip" && channeltosubs != null)
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

public class SkipTenChannelsVipQuery : SubscribeTenChannelsVipQuery
{
    public SkipTenChannelsVipQuery(Bot bot) : base(bot)
    {
        Names = new []{"/skipListedChannelVip"};
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return base.Run(context, cancellationToken);
    }
}

public class BlockTenChannelsVipQuery : SubscribeTenChannelsVipQuery
{
    public BlockTenChannelsVipQuery(Bot bot) : base(bot)
    {
        Names = new []{"/blockListedChannelVip"};
        MessageToSend = new string[] {
            "ShowChannel",
            "🤯 Благодарим! 🧐 Наша полиция нравов обязательно разберется с этим! \n\n" + "ShowChannel", 
            "🤯 Благодарим! 🧐 Наша полиция нравов обязательно разберется с этим! \n\n" +
                           "Вы слишком много раз нажали кнопку пропустить. Подпишитесь как минимум на десять" +
                            " каналов, предложенных выше."};
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        base.Run(context, cancellationToken, out Buttons);
        var channel = Database.GetChannel(context.Update.CallbackQuery.Message);
        if (channel != null)
        {
            channel.Reports += 1;
            channel.Update();
        }
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user.SubscribesVip?.Count > 5) //TODO: 20 in prod
        {
            Buttons.Clear(); //FIXME
            return MessageToSend[2];
        }
        return MessageToSend[1];
    }
}

class CheckSubscriptionsVip : SubscribeTenChannelsVipQuery, IListener
{
    private bool UserSubscribed = false;

    public CheckSubscriptionsVip(Bot bot) : base(bot)
    {
        Names = new[] { "/iSubscribedVip" };
        MessageToSend.Append("вы не подписаны на n, каналов, не надо так(");
        MessageToSend.Append("😉 Отлично, проверка прошла успешно! \n 🚨 ВАЖНО. \n Не отписывайся от этих каналов, " +
                            "иначе можно попасть в BlackList!");
    
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
            var userSubscribed = ChannelInfo.Subscribed(channelName: channel.Title, userId).Result;
            if (userSubscribed) UserSubscribed = true;
            if (UserSubscribed)
                totalAmount += 1;
        }
        if (totalAmount < 1) // TODO: prod - 10
        {
            return MessageToSend[2];
        }
        else
        {
            Buttons.Clear();
            Buttons.Add("Принято!", "/clear66step");
            return MessageToSend.Last();
                
        }
    }
    // public override async Task Handler(Context context, Dictionary<string, string> buttonsList, CancellationToken cancellationToken)
    // {
    //     base.Handler(context, buttonsList, cancellationToken);
    // }
}

