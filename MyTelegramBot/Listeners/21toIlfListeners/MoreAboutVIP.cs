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
            Globals.responses.GetValueOrDefault("WhatVipFor")
        };
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "cat.jpg", cancellationToken);
        buttons = new Dictionary<string, string>()
        {
            {Globals.responses.GetValueOrDefault("BuyLater"), "/buyVIPLater"},
            {Globals.responses.GetValueOrDefault("BuyNow"), "/buyVIPNow"}
        };
        return MessageToSend[0];
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
        return MessageToSend[0];
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
