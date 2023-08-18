using System.Security.Cryptography;
using Microsoft.Extensions.Localization;
using MongoDB.Bson.Serialization.Conventions;
using MyTelegramBot.Types;
using MyTelegramBot.Utils;
using Serilog;
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
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        var amount = "0.1";
        string link;
        CryproLinkParams cryproLinkParams = new(
                currencyCode: "TON",
                amount: amount,
                description: Globals.GetCommand("HundredForListing"),
                customData: context.Update.CallbackQuery.From.Id + "ListingPayload",
                externalId: "0", //TODO REMOVE!!!
                timeoutSeconds: 600,
                customerTelegramUserId: (int)context.Update.CallbackQuery.From.Id,
                WpayStoreApiKey: Environment.GetEnvironmentVariable("WpayStoreApiKey")
            );
        link = CryproLink.Get(cryproLinkParams);
        HandleParameters handleParameters = new();
        handleParameters.MessageToSend = Globals.GetCommand("HundredForListing");
        handleParameters.links.Add(Globals.GetCommand("PayViaWallet"), link);
        return handleParameters;
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
        MessageToSend = new string[] {Globals.GetCommand("ChannelAdded"),
                         Globals.GetCommand("ChooseChannelForListingButton")
        };
        
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        Log.Information("start running " + this.GetType());
        buttons = new();
        var messageLink = ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText;
        var userId = context.Update.CallbackQuery.From.Id;
        var user = Database.GetUser(userId);
        if (user.Channels?.Count > 1 && user.MainChannel == null) 
        {
            foreach(var channel in user.Channels)
            {
                var Title = Database.GetChannel(channel).Title;
                buttons.Add(Globals.GetCommand("ChooseChannelForListing"), $"/chooseChannelForListing {Title}");
            }
            return MessageToSend[1];
        }
        user.MainChannel = null;

        long chatId = Database.GetChannel(user.Channels.FirstOrDefault()).TelegramId;
        if(chatId == 0) throw new NullReferenceException("something wrong with DB");
        var messageParams = SplitReverse(messageLink, '/', 2);
        Log.Information("Bot start forwarding creative");
        try
        {
            Log.Information("Forward " + chatId + " " + messageParams[1].Replace("https://t.me/","@") + " " + messageParams[0]);
            context.BotClient.ForwardMessageAsync(
                chatId: chatId,
                fromChatId: messageParams[1].Replace("https://t.me/","@"),
                messageId: int.Parse(messageParams[0])
            ).Wait();
            Log.Information("creative forwarded");
        }
        catch(Exception ex)
        {
            Log.Error(ex.ToString());
            throw;
        }
        
        buttons = new Dictionary<string, string>()
        {
            { Globals.GetCommand("films"), "/watchMovies" },
            { Globals.GetCommand("books"), "/readBooks" }
        };
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "userhub.jpg", cancellationToken);
        return MessageToSend[0];
    }

    // start spliting from the end of the string
    public string[] SplitReverse(string message, char separator, int count)
    {
        return MakeReverseString(message).Split(separator, count).Select(x => MakeReverseString(x)).ToArray();
    }
    private string MakeReverseString(string message)
    {
        if (message.Count() > 1) return message = MakeReverseString(message.Remove(0, 1)) + message.First();
        else return message;
    }
}

public class ConfirmListingPayment : PayloadReply, IListener // TODO: make abstract listener for payments
{
    public ConfirmListingPayment(Bot bot) : base(bot)
    {
        Payload = "ListingPayload";
        MessageToSend = new[] {Globals.GetCommand("ChannelAdded")};
        
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            { Globals.GetCommand("films"), "/watchMovies" },
            { Globals.GetCommand("books"), "/readBooks" }
        };
        return MessageToSend[0];
    }
}