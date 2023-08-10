using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TL;
using LabeledPrice = Telegram.Bot.Types.Payments.LabeledPrice;

namespace MyTelegramBot.Listeners;

public class MoreAboutVIP : Query, IListener
{
    public MoreAboutVIP(Bot bot) : base(bot)
    {
        Names = new[] { "/moreAboutVIP" };
        MessageToSend = new[] {
                        "🎯 Для чего тебе статус VIP: \n" +
                        "1. Независимо от количества приглашенных в #UserHub пользователей, ты будешь получать подписчиков, " +
                        "так как канал будет находиться на верхних позициях выбранной категории каталога #UserHub, а также в" +
                        " выдаче среди другой 10-ки VIP-каналов, на которые подписываются пользователи при регистрации. \n" +
                        "2. Подписчики будут более целевыми, так как мы запрашиваем у пользователей, какие темы им интересны. \n " +
                        "3. На данный момент в боте ... пользователей, а также ... каналов, из которых ... в статусе VIP. " +
                        "В среднем каждый VIP получил по ... целевых подписчиков. \n " +
                        "4. Доступ в закрытый VIP-чат, где можно обмениваться опытом по раскрутке и монетизации своего канала.\n" +
                        "5. Возможность получать доход за счет программы #10рукопожатий #UserHub — по 5% с каждой оплаты " +
                        "ежемесячной и годовой подписки от всех VIP-пользователей на 10-и уровнях рукопожатий, а также платного " +
                        "листинга и всех других доходов #UserHub."
        };
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            {"Приобрести VIP 🏆 позже", "/buyVIPLater"},
            {"Приобрести статус VIP 🏆", "/buyVIPNow"}
        };
        return MessageToSend[0];
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "cat.jpg", cancellationToken);
        return base.Run(context, cancellationToken);
    }
}

public class BuyVIPLater : Query, IListener
{
    public BuyVIPLater(Bot bot) : base(bot)
    {
        Names = new[] { "/buyVIPLater" };
        MessageToSend = new[] {"Что-то я не нашел сообщения для такого случая..."};
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        //TODO: mb some logic to remind every N days/weeks?
        return base.Run(context, cancellationToken);
    }
}

public class BuyVIPNow : Query, IListener
{
    IEnumerable<LabeledPrice> prices;
    public BuyVIPNow(Bot bot) : base(bot)
    {
        Names = new[] { "/buyVIPNow" };
        MessageToSend = new[] {"Тут Сергей подкатывает платежку с применением @wallet. С меня кнопочка товара."};
        prices = new[]
        {
            new LabeledPrice("vipLabel", 10000)
        };
    }

    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        var invoiceAsync = await context.BotClient.SendInvoiceAsync(
            chatId: context.Update.CallbackQuery.Message.Chat.Id,
            title: "🏆 VIP статус на месяц",
            description: "Вип статус на месяц",
            payload: "VipMonthlyPayload",
            providerToken: Environment.GetEnvironmentVariable("providerToken"),
            currency: "RUB",
            prices: prices,
            cancellationToken: cancellationToken
            );
        
        // return base.Handler(context, cancellationToken);
    }
}

public class ConfirmVipPayment : Listener, IListener // TODO: make abstract listener for payments
{
    public ConfirmVipPayment(Bot bot) : base(bot)
    {
        
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

    public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.PreCheckoutQuery) return false;
        if (context.Update.PreCheckoutQuery.InvoicePayload == "VipMonthlyPayload") return true;
        return false;
    }
}
