using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;

namespace MyTelegramBot.Listeners;

public class PayForListingQuery : Query, IListener
{
    public PayForListingQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/payForListing" };
        Buttons = new Dictionary<string, string>() { {"Я отправил платеж на указаный адрес","/iPaid"} };
        MessageToSend = "👌 Хорошо! Что бы оплатить листинг кнеобходимо сделать перевод на 100 usdt в сети Bep-20 " +
                        "на адрес кошелька: 0x967dcty64su65.......... и отправить мне в сообщении хэш транзакции или" +
                        " адрес кошелька! 💡Важно! Любые активы отправленные в других сетях могут быть потеряны навсегда. ";
    }
}
public class BuyListingNow : Query, IListener
{
    IEnumerable<LabeledPrice> prices;
    public BuyListingNow(Bot bot) : base(bot)
    {
        Names = new[] { "/buyListingNow" };
        MessageToSend = "Тут Сергей подкатывает платежку с применением @wallet. С меня кнопочка товара.";
        prices = new[]
        {
            new LabeledPrice("Listing", 10000)
        };
    }

    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        var invoiceAsync = await context.BotClient.SendInvoiceAsync(
            chatId: context.Update.CallbackQuery.Message.Chat.Id,
            title: "Листинг",
            description: "Оплатить листинг канала",
            payload: "ListingPayload",
            providerToken: Environment.GetEnvironmentVariable("providerToken"),
            currency: "RUB",
            prices: prices,
            cancellationToken: cancellationToken
            );
        
        // return base.Handler(context, cancellationToken);
    }
}

public class ConfirmListingPayment : Listener, IListener // TODO: make abstract listener for payments
{
    public ConfirmListingPayment(Bot bot) : base(bot)
    {
        MessageToSend = "👋😎 Поздравляю, канал @jhvuy успешно добавлен в каталог! Добро пожаловать в комьюнити " +
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
                        "🧐 Кстати, что тебе нравится больше, смотреть фильмы или читать книги?";
        Buttons = new Dictionary<string, string>()
        {
            { "Смотреть фильмы", "/watchMovies" },
            { "Читать книги", "/readBooks" }
        };
    }

    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        var preCheckoutQueryId = context.Update.PreCheckoutQuery.Id;
        context.BotClient.AnswerPreCheckoutQueryAsync(
            preCheckoutQueryId: preCheckoutQueryId,
            cancellationToken: cancellationToken
        );
        //Todo: add node to DB
    }

    public override async Task Handler(
        Context context,
        Dictionary<string, string> dictionary,
        CancellationToken cancellationToken) 
    {
        var preCheckoutQueryId = context.Update.PreCheckoutQuery.Id;
        context.BotClient.AnswerPreCheckoutQueryAsync(
            preCheckoutQueryId: preCheckoutQueryId,
            cancellationToken: cancellationToken
        );
        
    }

    public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.PreCheckoutQuery) return false;
        if (context.Update.PreCheckoutQuery.InvoicePayload == "ListingPayload") return true;
        return false;
    }
}
