using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class PaymentProcessingQuery : Query
{
    public PaymentProcessingQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/iPaid" };
        MessageToSend = "😇 Ожидаю подтверждение платежа в блокчейне. Как только платеж будет получен я вас оповещу!";
    }
}