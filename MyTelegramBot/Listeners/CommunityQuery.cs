using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class CommunityQuery : Query, IListener
{
    public CommunityQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/UserhubCommunity" };
        MessageToSend = new string[]{Globals.GetCommand("Community")
        };
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            {Globals.GetCommand("AllHandshakes"), "/allHandshakes"},
            {Globals.GetCommand("FirstLevelHandshakes"), "/firstLevelHandshakes"},
        };
        return MessageToSend[0];
    }
}
