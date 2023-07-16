using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class GetAdressImAdminQuery : Query
{
    public GetAdressImAdminQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/admin" };
        Buttons= new Dictionary<string, string>(){{"Добавить канал", "/addChannel"}};
        MessageToSend = "🤩 Круто! Ты один из тех, кто 'создает' этот интернет! Предлагаю добавить твой " +
            "канал в ТОП 1 каталог каналов в Telegram. Это позволит тебе на постоянной основе получать целевых " +
            "подписчиков абсолютно бесплатно.";
    }
}