using System.ComponentModel;
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
            Globals.GetCommand("WhatVipFor")
        };
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "cat.jpg", cancellationToken);
        buttons = new Dictionary<string, string>()
        {
            {Globals.GetCommand("BuyLater"), "/buyVIPLater"},
            {Globals.GetCommand("BuyNow"), "/buyVIPNow"}
        };
        return MessageToSend[0];
    }
}

public class BuyVIPLater : Query, IListener
{
    public BuyVIPLater(Bot bot) : base(bot)
    {
        Names = new[] { "/buyVIPLater" };
        MessageToSend = new[] {Globals.GetCommand("Congratulations")};
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
        MessageToSend = new[] {Globals.GetCommand("YourRate")};
        prices = new[]
        {
            new LabeledPrice("vipLabel", 10000)
        };
    }
    protected virtual HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters handleParameters = new();
        var amount = "0.1";
        string link;
        CryproLinkParams cryproLinkParams = new(
                currencyCode: "TON",
                amount: amount,
                description: Globals.GetCommand("VipStatus"),
                customData: context.Update.CallbackQuery.From.Id + "ListingPayload",
                externalId: "0", //TODO REMOVE!!!
                timeoutSeconds: 600,
                customerTelegramUserId: (int)context.Update.CallbackQuery.From.Id,
                WpayStoreApiKey: Environment.GetEnvironmentVariable("WpayStoreApiKey")
            );
        link = CryproLink.Get(cryproLinkParams);
        handleParameters.MessageToSend = Globals.GetCommand("price50");
        handleParameters.links.Add(Globals.GetCommand("PayViaWallet"), link);
        return handleParameters;
    }
        // return base.Handler(context, cancellationToken);
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
