using MyTelegramBot;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

public class PayloadReply : Query
{
    public string Payload { get;set; } = "";
    public PayloadReply(Bot bot) : base(bot) { 

    }
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        base.Handler(context, cancellationToken);
        var preCheckoutQueryId = context.Update.PreCheckoutQuery.Id;
        context.BotClient.AnswerPreCheckoutQueryAsync(
                preCheckoutQueryId: preCheckoutQueryId,
                cancellationToken: cancellationToken
            );  
    }
    public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.PreCheckoutQuery) return false;
        if (context.Update.PreCheckoutQuery.InvoicePayload == Payload) return true;
        return false;
    }
}