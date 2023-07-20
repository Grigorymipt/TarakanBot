using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class MyHandshakesQuery : Query, IListener
{
    public MyHandshakesQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/myHandshakes" };
        MessageToSend = "🤝 Мои рукопожатия";
        Buttons = new Dictionary<string, string>()
        {
            {"🔍 Все рукопожатия", "/allHandshakes"},
            {"1-й уровень", "/firstLevelHandshakes"},
            {"Статистика", "/handshakeStats"},
        };
    }
}

public class AllHandshakesQuery : Query, IListener
{
    public AllHandshakesQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/allHandshakes" };
        MessageToSend = "🔍 Все рукопожатия: \n";
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var user = Database.GetUser(
            context.Update.CallbackQuery.From.Id
            );
        // TODO: рекурентный поиск по дереву
        return base.Run(context, cancellationToken);
    }
}
public class FirstLevelHandshakesQuery : Query, IListener
{
    public FirstLevelHandshakesQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/firstLevelHandshakes" };
        MessageToSend = "Рукопожатия первого уровня: \n";
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var user = Database.GetUser(
            context.Update.CallbackQuery.From.Id
        );
        var children = user.Children;
        foreach (var variableChild in children)
        {
            MessageToSend += variableChild + "\n";
        }
        
        return base.Run(context, cancellationToken);
    }
}