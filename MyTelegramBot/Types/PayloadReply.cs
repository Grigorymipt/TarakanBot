using MyTelegramBot;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

public class PayloadReply : Listener
{
    public string Payload { get;set; } = "";
    public PayloadReply(Bot bot) : base(bot) { 

    }
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        var preCheckoutQueryId = context.Update.PreCheckoutQuery.Id;
        await context.BotClient.AnswerPreCheckoutQueryAsync(
                preCheckoutQueryId: preCheckoutQueryId,
                cancellationToken: cancellationToken
            );   
        Int64 chantId = context.Update.PreCheckoutQuery.From.Id;
        await base.Handler(context, cancellationToken);
    }
    public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.PreCheckoutQuery) return false;
        if (context.Update.PreCheckoutQuery.InvoicePayload == Payload) return true;
        return false;
    }
}