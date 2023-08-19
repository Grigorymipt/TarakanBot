using System.Collections;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using TL;
using InlineQuery = MyTelegramBot.Types.InlineQuery;
namespace MyTelegramBot.Listeners;

public class WhenQuery : Query, IListener
{
    public WhenQuery(Bot bot): base(bot) {
        Names = new string[]{"/when"};
    }
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters handleParameters = new();
        var arguments = ArgumentParser.Parse(context.Update.CallbackQuery.Data).Arguments;
        Place place = PlaceStruct.GetPlace(arguments.First());
        if(arguments.Last() == "yes" && 1==0) //TODO: remove second condition
        {
            handleParameters.MessageToSend = Globals.GetCommand("SendTime");
            foreach(Time time in Enum.GetValues(typeof(Time)))
            {
                handleParameters.buttons.Add(Globals.GetCommand(time.ToString()), $"/time {place.ToString()} {time.ToString()}");
            }    
        }
        else
        {
            handleParameters.MessageToSend = Globals.GetCommand("Congratulations");
            var user = Database.GetUser(context.Update.Message).Result;
            if (user == null) throw new NullReferenceException("User not found");
            Tarakan tarakan = new(place, DateTime.Now);
            user.Tarakans.Add(tarakan.Id);
        }
        return handleParameters;
    }
}