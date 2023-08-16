
using Telegram.Bot.Types.Payments;
using MyTelegramBot.Types;


namespace MyTelegramBot.Listeners;

public class PayInCrypto : Query, IListener
{
    IEnumerable<LabeledPrice> prices;
    public PayInCrypto(Bot bot) : base(bot)
    {
        Names = new[] { "/payInCrypto" };
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
}