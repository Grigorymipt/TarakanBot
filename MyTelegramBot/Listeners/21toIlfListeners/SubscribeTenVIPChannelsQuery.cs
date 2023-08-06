using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = MongoDatabase.ModelTG.User;

namespace MyTelegramBot.Listeners._21toIlfListeners;

public class SubscribeTenVIPChannelsQuery : Query, IListener
{
    protected List<string> ChannelNames = new List<string>() { "google.com" };
    protected string ChannelName = "https://t.me/TestForTestingAndTestingForTest";
    public SubscribeTenVIPChannelsQuery(Bot bot) : base(bot)
    {
        MessageToSend = "Some @ - channel with short description, EX: " + ChannelName;
        Names = new[] { "/subscribeTenVIPChannels" };
        //Links = ...
        Buttons = new Dictionary<string, string>()
        {
            { "🟢 Подписаться", "/subscribeListedVIPChannel" }, // MakeLink
            { "🔴 Пропустить", "/skipListedVIPChannel" },
            // { "🔴 Black List 🔴", "/blockListedVIPChannel " + ChannelName },
            { "Подписался на 10 каналов", "/iSubscribedVIP" }
        };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user.SubscribesVip > 5) //TODO: 20 in prod
        {
            MessageToSend = "Вы слишком много раз нажали кнопку пропустить. Подпишитесь как минимум на десять" +
                            " каналов, предложенных выше.";
            Buttons.Clear(); //FIXME
        }
        return base.Run(context, cancellationToken);
    }

    public override async Task Handler(Context context, Dictionary<string, string> buttonsList, CancellationToken cancellationToken)
    {
        string response = await RunAsync(context, cancellationToken);
        Int64 chatId = context.Update.CallbackQuery.Message.Chat.Id;

        List<IEnumerable<InlineKeyboardButton>> categoryList = new List<IEnumerable<InlineKeyboardButton>>();
        foreach (var category in buttonsList)
        {
            InlineKeyboardButton reply;
            if (category.Value == "/subscribeListedVIPChannel" )
            {
                reply = InlineKeyboardButton
                    .WithUrl(category.Key, ChannelName);
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

public class SkipTenVIPChannelsQuery : SubscribeTenVIPChannelsQuery, IListener
{
    public SkipTenVIPChannelsQuery(Bot bot) : base(bot)
    {
        Names = new []{"/skipListedVIPChannel"};
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        user.SubscribesVip += 1;
        user.Update();
        return base.Run(context, cancellationToken);
    }
}

public class BlockTenVIPChannelsQuery : SubscribeTenVIPChannelsQuery, IListener
{
    public BlockTenVIPChannelsQuery(Bot bot) : base(bot)
    {
        Names = new []{"/blockListedVIPChannel"};
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
        if (user.Subscribes.Count > 5) //TODO: 20 in prod
        {
            MessageToSend ="🤯 Благодарим! 🧐 Наша полиция нравов обязательно разберется с этим! \n\n" +
                           "Вы слишком много раз нажали кнопку пропустить. Подпишитесь как минимум на десять" +
                            " каналов, предложенных выше.";
            Buttons.Clear(); //FIXME
        }
        return MessageToSend;
    }
}

class CheckSubscriptionsVIP : Query, IListener
{
    private bool UserSubscribed = false;

    public CheckSubscriptionsVIP(Bot bot) : base(bot)
    {
        Names = new[] { "/iSubscribedVIP" };
        Buttons = new Dictionary<string, string>();
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var userId = context.Update.CallbackQuery.From.Id;
        var userSubscribed = ChannelInfo.Subscribed(channelName: "TestForTestingAndTestingForTest", userId).Result;
        if (userSubscribed) UserSubscribed = true;
        if (UserSubscribed)
        {
            MessageToSend = "😉 Отлично, проверка прошла успешно! \n 🚨 ВАЖНО. \n Не отписывайся от этих каналов, " +
                            "иначе можно попасть в BlackList!";
            Buttons.Clear();
            Buttons.Add("Принято!", "/clear66step");
        }
        else
        {
            MessageToSend = "Тут подтягиваем логику проверки подписки и: вы не подписаны на n, каналов, не надо так(";
            //TODO: logics
        }

        return base.Run(context, cancellationToken);
    }
}
