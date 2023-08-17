using MyTelegramBot.Types;
using Serilog;

namespace MyTelegramBot.Listeners;

public class MyVipStatusQuery : Query, IListener
{
    public MyVipStatusQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/myVipStatus" };
        MessageToSend = new string[]{Globals.GetCommand("VipChooseChannel")
        };
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>();
        foreach (var variableChannel in Database.GetUser(context.Update.CallbackQuery.From.Id).Channels)
        {
            buttons.Add(variableChannel, "/getVipStatusForChannel " + variableChannel);
        }
        return MessageToSend[0];
    }
}

public class GetVipStatusFotChannelQuery : Query, IListener
{
    public GetVipStatusFotChannelQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/getVipStatusForChannel" };
        
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>();
        var MessageToSend = base.MessageToSend[0];

        var contextupdate = context.Update.CallbackQuery.Data;
        Console.WriteLine(contextupdate);
        Log.Information(contextupdate);
        Console.WriteLine(ArgumentParser.Parse(contextupdate).ArgumentsText);
        Log.Information(ArgumentParser.Parse(contextupdate).ArgumentsText);
        var channel = Database.GetChannel(
            ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText);
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        
        if (channel != null)
        {
            if (channel.Vip.CompareTo(today) < 0)
            {
                MessageToSend = Globals.GetCommand("ChooseRate");
                Buttons.Add(Globals.GetCommand("FiftyMonth"), "/payForMonth");//TODO: опять же платежка сергея
                Buttons.Add(Globals.GetCommand("FiveHundYear"),  "/payForYear");
            }
            else
            {
                MessageToSend = Globals.GetCommand("VipInfo") + channel.Vip +
                                Globals.GetCommand("RecPaySug");
            }
        }
        return MessageToSend;
    }
}