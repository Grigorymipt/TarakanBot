using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class PayForListingQuery : Query
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