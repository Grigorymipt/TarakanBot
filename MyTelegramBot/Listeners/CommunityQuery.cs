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
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters handleParameters = new HandleParameters();
        handleParameters.MessageToSend = MessageToSend[0];

        handleParameters.links.Add(
            Globals.GetCommand("CommunityChannel"), 
            Globals.GetCommand("CommunityChannelLink"));
            
        handleParameters.links.Add(
            Globals.GetCommand("CommunityChat"),
            Globals.GetCommand("CommunityChatLink"));
        
        return handleParameters;
    }
}
