using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class MyHandshakesQuery : Query, IListener
{
    public MyHandshakesQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/myHandshakes" };
        MessageToSend = new string[]{"🤝 Мои рукопожатия"
        };
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            {"🔍 Все рукопожатия", "/allHandshakes"},
            {"1-й уровень", "/firstLevelHandshakes"},
            {"Статистика", "/handshakeStats"},
        };
        return MessageToSend[0];
    }
}

public class AllHandshakesQuery : Query, IListener
{
    public AllHandshakesQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/allHandshakes" };
        // MessageToSend = "🔍 Все рукопожатия: \n";
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var MessageToSend = "🕹 Уровень | Рукопожатий | VIP\n";
        Dictionary<int, int> handshakes = new Dictionary<int, int>();
        // итеративный листинг дерева
        Queue<string> queue = new Queue<string>();
        HashSet<string> visited = new HashSet<string>();
        
        var user = Database.GetUser(
            context.Update.CallbackQuery.From.Id
        );
        int depth = 0;
        int width = 1;
        int nextWidth = 0;
        handshakes.Add(depth, user.Children.Count);
        queue.Enqueue(user.UserName);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            visited.Add(node);
            var nodeUser = Database.GetUser(node);
            var nodesChildren = Database.GetUser(node).Children;
            nextWidth += nodesChildren.Count;
            foreach (var child in nodesChildren)
            {
                if (!visited.Contains(child))
                {
                    queue.Enqueue(child);
                    visited.Add(child);
                }
            }
            width -= 1;
            if (width == 0)
            {
                depth += 1;
                width = nextWidth;
                nextWidth = 0;
                handshakes.Add(depth, nextWidth);
                MessageToSend += depth + " | " + width + " | " + 0 + "\n"; //TODO: not null but vips
            }
            if (depth > 10) queue.Clear();
        }

        MessageToSend += $"🏆 Всего на {depth} уровнях рукопожатий:\n"+
                         $"{visited.Count} пользователей\n"+
                         $"{0} VIP";
        
        return MessageToSend;
    }
}

public class FirstLevelHandshakesQuery : Query, IListener
{
    public FirstLevelHandshakesQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/firstLevelHandshakes" };
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var MessageToSend = "🧑‍💻 Ник | Всего Рукопожатий | VIP";
        var user = Database.GetUser(
            context.Update.CallbackQuery.From.Id
        );
        var children = user.Children;
        foreach (var variableChild in children)
            MessageToSend += $"@{variableChild} | allHandshakesOfNik | Vips\n";
        return MessageToSend;
    }
}

