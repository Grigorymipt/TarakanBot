using System.Collections;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Listeners;

public class CatalogCommand : Query, IListener
{
    public CatalogCommand(Bot bot): base(bot) {
        Names = new string[]{"/catalog"};
    }
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters handleParameters = new();
        foreach (Place place in Enum.GetValues(typeof(Place)))
        {        
            handleParameters.buttons.Add(Globals.GetCommand(place.ToString()), $"/stats {place.ToString()}");
        }
        handleParameters.MessageToSend = Globals.GetCommand("Catalog");
        return handleParameters;
    }
}


