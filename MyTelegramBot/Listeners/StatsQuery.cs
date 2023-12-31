using System.Collections;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Listeners;

public class StatsQuery : Query, IListener
{
    public StatsQuery(Bot bot): base(bot) {
        Names = new string[]{"/stats"};
    }
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters handleParameters = new();
        Place place = PlaceStruct.GetPlace(ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText);
        List<Tarakan> documents = Database.GetDocumentMany<Tarakan, int>(u => u.place, (int)place).Result;
        handleParameters.MessageToSend = Globals.GetCommand("Catalog") + documents.Count();
        return handleParameters;
    }
}