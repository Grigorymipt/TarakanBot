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
        MessageToSend = "Выберите временной период";
        Buttons = new Dictionary<string, string>()
        {
            { "Сегодня", "/handshakeStatsFor " + Period.Day},
            { "Неделя", "/handshakeStatsFor " + Period.Week},
            { "Месяц", "/handshakeStatsFor " + Period.Month},
            { "Квартал", "/handshakeStatsFor " + Period.Quarter},
            { "Год", "/handshakeStatsFor " + Period.Year},
            { "Все время", "/handshakeStatsFor " + Period.All},
        };
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
        MessageToSend = "Ваши рукопожатия за " + ArgumentParser.Parse(
            context.Update.CallbackQuery.Data).ArgumentsText;
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        switch (ArgumentParser.Parse(
                    context.Update.CallbackQuery.Data).ArgumentsText)
        {
            case "All":
                foreach (var child in user.Children)
                {
                    Console.WriteLine(child);
                }
                break; 
            // TODO: Add All Periods
        }
        //Logics
        return base.Run(context, cancellationToken);
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