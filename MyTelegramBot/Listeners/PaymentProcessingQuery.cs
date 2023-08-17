using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class PaymentProcessingQuery : Query, IListener
{
    public PaymentProcessingQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/iPaid" };
        MessageToSend = new string[]{Globals.GetCommand("WaitForCryptoPayment")};
    }
}