using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners._21toIlfListeners;

public class StartTest : Query, IListener
{
    public StartTest(Bot bot) : base(bot)
    {
        Names = new[] { "/startTest" };
        MessageToSend = "🟢 Сколько пользователей ChatGPT получил за первые 2 месяца благодаря 'сарафанному радио'?";
        Buttons = new Dictionary<string, string>()
        {
            {"1 млн.", "/oneMillion"},
            {"10 млн.", "/tenMillions"},
            {"100 млн.", "/hundredMillions"}
        };
    }
}

public class WrongTestAnswer : StartTest
{
    public WrongTestAnswer(Bot bot) : base(bot)
    {
        Names = new[] { "/oneMillion", "/tenMillions" };
        MessageToSend = "🙅‍♂ Неверно, попробуй еще раз!";
    }
}
