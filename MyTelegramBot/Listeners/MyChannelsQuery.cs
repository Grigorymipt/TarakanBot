using Amazon.Runtime.Internal.Transform;
using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class MyChannelsQuery : Query, IListener
{
    public MyChannelsQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/myChannels" };
        MessageToSend = "Мои каналы";
        Buttons = new Dictionary<string, string>(){{"Добавить канал", "/addChannel"}};
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        foreach (var variableChannel 
                 in Database.GetUser(context.Update.CallbackQuery.From.Id).Channels)
        {
            Buttons.Add(variableChannel, "/chooseChannel " + variableChannel);
        }
        return base.Run(context, cancellationToken);
    }
}

public class ChooseChannelQuery : Query, IListener
{
    public ChooseChannelQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/chooseChannel" };
        MessageToSend = "Канал ";
        Buttons = new Dictionary<string, string>()
        {
            { "Поменять канал", "/changeChannel" },
            {"Статистика", "/handshakeStats"}, // TODO: not handshake
        };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var channelName = ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText;
        MessageToSend += channelName;
        Buttons.Add("VIP статус", "/getVipStatusForChannel " + channelName);
        return base.Run(context, cancellationToken);
    }
}

public class ChangeChannelQuery : Query, IListener
{
    public ChangeChannelQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/changeChannel" };
        MessageToSend = "💡Поменять канал можно только в том случае, если у вас изменился адрес канала. При этом " +
                        "канал остался прежним. Для этого необходимо связаться с тех. поддержкой #UserHub  -----> @uchjhi";

    }
}