using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class MyVipStatusQuery : Query, IListener
{
    public MyVipStatusQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/myVipStatus" };
        MessageToSend = "🏆 VIP статус\n" +
                        "Выберите канал:";
        Buttons = new Dictionary<string, string>();
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        foreach (var variableChannel in Database.GetUser(context.Update.CallbackQuery.From.Id).Channels)
        {
            Buttons.Add(variableChannel, "/getVipStatusForChannel " + variableChannel);
        }
        return base.Run(context, cancellationToken);
    }
}

public class GetVipStatusFotChannelQuery : Query, IListener
{
    public GetVipStatusFotChannelQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/getVipStatusForChannel" };
        Buttons = new Dictionary<string, string>();
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var channel = Database.GetChannel(
            ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText);
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (channel != null)
        {
            if (channel.Vip.CompareTo(today) < 0)
            {
                MessageToSend = "🏆 Для активации VIP необходимо выбрать тариф";
                Buttons.Add("$50 в месяц", "/payForMonth");//TODO: опять же платежка сергея
                Buttons.Add("$500 в год",  "/payForYear");
            }
            else
            {
                MessageToSend = $"🏆 VIP статус активирован до {channel.Vip}. \n" +
                                "💡 Рекомендую включить авто продление. \n" +
                                "Если ни один канал не имеет статус VIP, то можно потерять партнерские начисления!\n";
            }
        }
        return base.Run(context, cancellationToken);
    }
}