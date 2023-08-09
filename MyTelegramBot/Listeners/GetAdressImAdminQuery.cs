using System.Threading;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = MongoDatabase.ModelTG.User;

namespace MyTelegramBot.Listeners;

public class  GetAdressImAdminQuery : Query, IListener
{
    public GetAdressImAdminQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/admin" };
        Buttons= new Dictionary<string, string>(){{"Добавить канал", "/addChannel"}};
        MessageToSend = "🤩 Круто! Ты один из тех, кто 'создает' этот интернет! Предлагаю добавить твой " +
            "канал в ТОП 1 каталог каналов в Telegram. Это позволит тебе на постоянной основе получать целевых "+
        "подписчиков абсолютно бесплатно.";
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "admin.jpg", cancellationToken);
        return base.Run(context, cancellationToken);
    }
}