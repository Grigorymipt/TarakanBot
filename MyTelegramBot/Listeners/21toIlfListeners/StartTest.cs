using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners._21toIlfListeners;

public class StartTest : Query, IListener
{
    public StartTest(Bot bot) : base(bot)
    {
        Names = new[] { "/startTest" };
        MessageToSend = new[] {"🟢 Сколько пользователей ChatGPT получил за первые 2 месяца благодаря 'сарафанному радио'?"};
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            {"1 млн.", "/oneMillion"},
            {"10 млн.", "/tenMillions"},
            {"100 млн.", "/hundredMillions"}
        };
        return MessageToSend[0];
    }
}

public class WrongTestAnswer : StartTest, IListener
{
    public WrongTestAnswer(Bot bot) : base(bot)
    {
        Names = new[] { "/oneMillion", "/tenMillions" };
        MessageToSend = new[] {"🙅‍♂ Неверно, попробуй еще раз!"};
    }
}
