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
    protected List<string> ChannelNames = new List<string>() { "google.com" };
    protected string ChannelName = "https://t.me/TestForTestingAndTestingForTest";
    public SubscribeTenChannelsQuery(Bot bot) : base(bot)
    {
        MessageToSend = "Some @ - channel with short description, EX: " + ChannelName;
        Names = new[] { "/subscribeTenChannels" };
        //Links = ...
        Buttons = new Dictionary<string, string>()
        {
            { "🟢 Подписаться", "/subscribeListedChannel" }, // MakeLink
            { "🔴 Пропустить", "/skipListedChannel" },
            { "🔴 Black List 🔴", "/blockListedChannel " + ChannelName },
            { "Подписался на 10 каналов", "/iSubscribed" }
        };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user.Subscribes > 5) //TODO: 20 in prod
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
            if (category.Value == "/subscribeListedChannel" )
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

public class SkipTenChannelsQuery : SubscribeTenChannelsQuery
{
    public SkipTenChannelsQuery(Bot bot) : base(bot)
    {
        Names = new []{"/skipListedChannel"};
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        user.Subscribes += 1;
        user.Update();
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
        if (user.Subscribes > 5) //TODO: 20 in prod
        {
            MessageToSend ="🤯 Благодарим! 🧐 Наша полиция нравов обязательно разберется с этим! \n\n" +
                           "Вы слишком много раз нажали кнопку пропустить. Подпишитесь как минимум на десять" +
                            " каналов, предложенных выше.";
            Buttons.Clear(); //FIXME
        }
        return MessageToSend;
    }
}

class CheckSubscriptions : Query, IListener
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
        var userSubscribed = ChannelInfo.Subscribed(channelName: "TestForTestingAndTestingForTest", userId).Result;
        if (userSubscribed) UserSubscribed = true;
        if (UserSubscribed)
        {
            MessageToSend = "🎯 Отлично, теперь необходимо подписаться на 10 каналов VIP блоггеров. При нажатии на " +
                            "кнопку пропустить канал будет заменен на другой, исходя из указанных интересов. " +
                            "Нажать 'пропустить' можно не более 20 раз. 🚨🚔 Если канал нарушает правила пользования " +
                            "#UserHub, то жми «пропустить» а затем «Black List» и наши специалисты разберутся с этим.";
            Buttons.Clear();
            Buttons.Add("Принято!", "/subscribeTenVIPChannels");
        }
        else
        {
            MessageToSend = "Тут подтягиваем логику проверки подписки и: вы не подписаны на n, каналов, не надо так(";
            //TODO: logics
        }

        return base.Run(context, cancellationToken);
    }
}
