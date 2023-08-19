using System.Collections;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using TL;
using InlineQuery = MyTelegramBot.Types.InlineQuery;
namespace MyTelegramBot.Listeners;

public class TimeQuery : Query, IListener
{
    public TimeQuery(Bot bot): base(bot) {
        Names = new string[]{"/time"};
    }
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters handleParameters = new();

        var user = Database.GetUser(context.Update.Message).Result;
        if (user == null) throw new NullReferenceException("User not found");

        var arguments = ArgumentParser.Parse(context.Update.CallbackQuery.Data).Arguments;
        Place place = PlaceStruct.GetPlace(arguments.First());

        Tarakan tarakan = new(place, DateTime.Now);
        user.Tarakans.Add(tarakan.Id);
        return handleParameters;
    }
}