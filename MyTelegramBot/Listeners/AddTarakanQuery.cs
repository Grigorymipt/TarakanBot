using System.Collections;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Listeners;

public class AddTarakanQuery : Query, IListener
{
    public AddTarakanQuery(Bot bot): base(bot) {
        Names = new string[]{"/addTarakan"};
    }
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters handleParameters = new();
        handleParameters.MessageToSend = Globals.GetCommand("Where");
        foreach (Place place in Enum.GetValues(typeof(Place)))
        {        
            handleParameters.buttons.Add(Globals.GetCommand(place.ToString()), $"/where {place.ToString()}");
        }
        return handleParameters;
    }
}
