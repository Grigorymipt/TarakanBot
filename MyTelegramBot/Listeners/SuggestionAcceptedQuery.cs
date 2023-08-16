using MyTelegramBot.Types;
using Serilog;
using Telegram.Bot;

namespace MyTelegramBot.Listeners;

public class SuggestionAcceptedQuery : Query, IListener
{
    Dictionary<string, string> creatives = new(); // read from file
    public SuggestionAcceptedQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/suggestionAccepted" };
        

        //add logics to get creatives link
        creatives.Add("Креатив 1", "https://t.me/vokyj/5");
        MessageToSend = new string[] {"👍 Правильный выбор! Выбери один из предложенных креативов, после чего нажми соответствующую кнопку:"
            + " наш бот сделает репост этого креатива в твой канал, после чего твой канал автоматически появится в каталоге #UserHub!"
                        };
        foreach (var creative in creatives)
        {
            MessageToSend[0] += "\n" + creative.Key + ": " + creative.Value;
        }
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>();
        Log.Information($"Adding creatives: {creatives.Count()}");
        foreach(var creative in creatives)
        {
            buttons.Add("Продолжить1", $"/whatLike {creative.Value}");
            Log.Information($"add creative {creative} to buttons list");
        }
        Log.Information("smth");
        return MessageToSend[0];
    }
}



public class ExampleQuery : Query, IListener
{
    Dictionary<string, string> creatives = new(); // read from file
    public ExampleQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/exampleQuery" };
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>();
        buttons.Add("some button", "/commandToRedirect");
        return "some text";
    }
}