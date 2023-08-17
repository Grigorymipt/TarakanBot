using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners._21toIlfListeners;

public class StartTest : Query, IListener
{
    public StartTest(Bot bot) : base(bot)
    {
        Names = new[] { "/startTest" };
        MessageToSend = new[] {Globals.responses.GetValueOrDefault("gpt")};
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            {Globals.responses.GetValueOrDefault("onemill"), "/oneMillion"},
            {Globals.responses.GetValueOrDefault("tenmillions"), "/tenMillions"},
            {Globals.responses.GetValueOrDefault("hundredmillions"), "/hundredMillions"}
        };
        return MessageToSend[0];
    }
}

public class WrongTestAnswer : StartTest, IListener
{
    public WrongTestAnswer(Bot bot) : base(bot)
    {
        Names = new[] { "/oneMillion", "/tenMillions" };
        MessageToSend = new[] {Globals.responses.GetValueOrDefault("wrongagain")};
    }
}
