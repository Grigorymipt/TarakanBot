using Microsoft.Extensions.Localization;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Listeners;

public class PayForListingQuery : Query, IListener
{
    public PayForListingQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/payForListing" };
        MessageToSend = new string[] {"👌 Хорошо! Что бы оплатить листинг кнеобходимо сделать перевод на 100 usdt в сети Bep-20 " +
                        "на адрес кошелька: 0x967dcty64su65.......... и отправить мне в сообщении хэш транзакции или" +
                        " адрес кошелька! 💡Важно! Любые активы отправленные в других сетях могут быть потеряны навсегда. "
                        };
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>() { {"Я отправил платеж на указаный адрес","/iPaid"} };
        return MessageToSend[0];
    }
}
public class BuyListingNow : Query, IListener
{
    IEnumerable<LabeledPrice> prices;
    public BuyListingNow(Bot bot) : base(bot)
    {
        Names = new[] { "/buyListingNow" };
        MessageToSend = new string[] {"Тут Сергей подкатывает платежку с применением @wallet. С меня кнопочка товара."
        };
        prices = new[]
        {
            new LabeledPrice("Listing", 10000)
        };
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return MessageToSend[0];
    }
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        var amount = 0.01;
        var link = await Crypto.CreateOrder.PostAsync(
            "TON",
            amount.ToString(),
            "some description",
            customData: context.Update.CallbackQuery.From.Id + "ListingPayload",
            externalId: "0", //TODO REMOVE!!!
            timeoutSeconds: 120,
            customerTelegramUserId: (int)context.Update.CallbackQuery.From.Id,
            WpayStoreApiKey: Environment.GetEnvironmentVariable("WpayStoreApiKey")
        );
        var buttons = new Dictionary<string, string>(){};
        string response = Task.Run(() => Run(context, cancellationToken, out buttons)).Result;
        Int64 chatId = context.Update.CallbackQuery.Message.Chat.Id;
        List<IEnumerable<InlineKeyboardButton>> categoryList = new List<IEnumerable<InlineKeyboardButton>>();

        InlineKeyboardButton reply;
        try
        {
            // Console.WriteLine("");
            reply = InlineKeyboardButton
                .WithUrl("Оплатить", link);
        }
        catch(Exception ex)
        {
            throw ex;
        }

        IEnumerable<InlineKeyboardButton> inlineKeyboardButton = new[] { reply };
        categoryList.Add(inlineKeyboardButton);

        IEnumerable<IEnumerable<InlineKeyboardButton>> enumerableList1 = categoryList;
        InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(enumerableList1);

        Message sentMessage = await context.BotClient.SendTextMessageAsync(
            chatId: chatId,
            text: response,
            parseMode: Config.ParseMode,
            replyMarkup: inlineKeyboardMarkup
        );
    }


        // var invoiceAsync = await context.BotClient.SendInvoiceAsync(
        //     chatId: context.Update.CallbackQuery.Message.Chat.Id,
        //     title: "Листинг",
        //     description: "Оплатить листинг канала",
        //     payload: "ListingPayload",
        //     providerToken: Environment.GetEnvironmentVariable("providerToken"),
        //     currency: "RUB",
        //     prices: prices,
        //     cancellationToken: cancellationToken
        //     );
    
}

public class ContinueToRW : Query, IListener // TODO: make abstract listener for payments
{
    public ContinueToRW(Bot bot) : base(bot)
    {
        Names = new[] {"/whatLike"};
        MessageToSend = new string[] {
                        "👋😎 Поздравляю, канал @jhvuy успешно добавлен в каталог! Добро пожаловать в комьюнити " +
                        "блогеров Telegram. С помощью #UserHub ты сможешь: \n- многократно увеличить количество " +
                        "читателей и охваты в своем Telegram-канале, или быстро раскрутить Telegram-канал с нуля; \n" +
                        "- узнать о новых способах монетизации и применить их в своем канале; \n- общаться в закрытом " +
                        "чате с другими блогерами, обмениваться опытом, создавать совместные проекты; \n" +
                        "- получать ежемесячный высокий доход благодаря системе #10рукопожатий #UserHub \n " +
                        "👀 Также, в ближайшем будущем:\n" +
                        "- безопасно покупать и продавать рекламные интеграции;\n" +
                        "- пользоваться услугами гаранта для безопасной сделки при покупке или продаже канала;\n" +
                        "- найти копирайтера, дизайнера или менеджера по рекламе в свою команду;\n" +
                        "- получать партнёрское вознаграждение от всех комиссий на новые продукты сервиса #UserHub.\n" +
                        "🕹 Пользоваться сервисом можно абсолютно бесплатно, но для этого тебе необходимо " +
                        "пройти небольшой квест. Обещаю, уже на следующем шаге я раскрою секрет как получить " +
                        "первый 1.000.000 подписчиков на свой Telegram-канал не вложив ни рубля. \n" +
                        "🧐 Кстати, что тебе нравится больше, смотреть фильмы или читать книги?",
                        "Ты не репостнул запись, или проверка еще не прошла",
        };
        
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        if(ChannelInfo.CheckMessageAutor("TestForTestingAndTestingForTest", 5, 6).Result) 
        {
            buttons = new Dictionary<string, string>()
            {
                { "Смотреть фильмы", "/watchMovies" },
                { "Читать книги", "/readBooks" }
            };
            Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "userhub.jpg", cancellationToken);
            return MessageToSend[0];
        }
        buttons = new Dictionary<string, string>();
        buttons.Add("Повторить проверку репоста", "/whatLike");
        return MessageToSend[1];
    }
}
public class ConfirmListingPayment : PayloadReply, IListener // TODO: make abstract listener for payments
{
    public ConfirmListingPayment(Bot bot) : base(bot)
    {
        Payload = "ListingPayload";
        MessageToSend = new[] {
            "👋😎 Поздравляю, канал @jhvuy успешно добавлен в каталог! Добро пожаловать в комьюнити " +
            "блогеров Telegram. С помощью #UserHub ты сможешь: \n- многократно увеличить количество " +
            "читателей и охваты в своем Telegram-канале, или быстро раскрутить Telegram-канал с нуля; \n" +
            "- узнать о новых способах монетизации и применить их в своем канале; \n- общаться в закрытом " +
            "чате с другими блогерами, обмениваться опытом, создавать совместные проекты; \n" +
            "- получать ежемесячный высокий доход благодаря системе #10рукопожатий #UserHub \n " +
            "👀 Также, в ближайшем будущем:\n" +
            "- безопасно покупать и продавать рекламные интеграции;\n" +
            "- пользоваться услугами гаранта для безопасной сделки при покупке или продаже канала;\n" +
            "- найти копирайтера, дизайнера или менеджера по рекламе в свою команду;\n" +
            "- получать партнёрское вознаграждение от всех комиссий на новые продукты сервиса #UserHub.\n" +
            "🕹 Пользоваться сервисом можно абсолютно бесплатно, но для этого тебе необходимо " +
            "пройти небольшой квест. Обещаю, уже на следующем шаге я раскрою секрет как получить " +
            "первый 1.000.000 подписчиков на свой Telegram-канал не вложив ни рубля. \n" +
            "🧐 Кстати, что тебе нравится больше, смотреть фильмы или читать книги?"};
        
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            { "Смотреть фильмы", "/watchMovies" },
            { "Читать книги", "/readBooks" }
        };
        return MessageToSend[0];
    }
}