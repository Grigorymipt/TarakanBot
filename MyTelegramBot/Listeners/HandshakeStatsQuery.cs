using MongoDB.Bson;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBot.Listeners;

public class HandshakeStatsQuery : Query, IListener
{
    public HandshakeStatsQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/handshakeStats" };
        MessageToSend = new string[] {Globals.GetCommand("ChooseTimePeriod")};
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            { Globals.GetCommand("Day"), "/handshakeStatsFor " + Period.Day},
            { Globals.GetCommand("Week"), "/handshakeStatsFor " + Period.Week},
            { Globals.GetCommand("Month"), "/handshakeStatsFor " + Period.Month},
            { Globals.GetCommand("Quarer"), "/handshakeStatsFor " + Period.Quarter},
            { Globals.GetCommand("Year"), "/handshakeStatsFor " + Period.Year},
            { Globals.GetCommand("AllTime"), "/handshakeStatsFor " + Period.All},
        };
        return MessageToSend[0];
    }
}

public class HandshakeStatsForQuery : Query, IListener
{
    public HandshakeStatsForQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/handshakeStatsFor" };
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var MessageToSend = Globals.GetCommand("YourHandshakesFor") + ArgumentParser.Parse(
            context.Update.CallbackQuery.Data).ArgumentsText;
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        var currentTime = DateTime.Now;
        var quantity = 0;
        switch (ArgumentParser.Parse(
                    context.Update.CallbackQuery.Data).ArgumentsText)
        {
            case "All":
                quantity = Database.ListChildrenFrom(user.UserName, DateTime.MinValue).Count;
                break; 
            case "Year" :
                quantity = Database.ListChildrenFrom(user.UserName, new DateTime(currentTime.Year - 1, currentTime.Month, currentTime.Day)).Count;
                break;
            case "Month" :
                quantity = Database.ListChildrenFrom(user.UserName, new DateTime(currentTime.Year, currentTime.Month - 1, currentTime.Day)).Count;
                break;
            case "Quarter" :
                quantity = Database.ListChildrenFrom(user.UserName, new DateTime(currentTime.Year, currentTime.Month - 3, currentTime.Day)).Count;
                break;
            case "Week" :
                quantity = Database.ListChildrenFrom(user.UserName, new DateTime(currentTime.Year, currentTime.Month, currentTime.Day - 7)).Count;
                break;
            case "Day" :
                quantity = Database.ListChildrenFrom(user.UserName, new DateTime(currentTime.Year, currentTime.Month, currentTime.Day - 1)).Count;
                break;         
        }
        //Logics
        return MessageToSend + quantity;
    }
}
public enum Period 
{
    Day,
    Week,
    Month,
    Quarter,
    Year,
    All,
}