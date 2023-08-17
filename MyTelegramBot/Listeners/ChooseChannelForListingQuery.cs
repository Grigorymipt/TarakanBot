using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class ChooseChannelForListingQuery : Query, IListener
{
    public ChooseChannelForListingQuery(Bot bot): base(bot) {
        Names = new[] {"/chooseChannelForListing"};
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>();
        var channelName = ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText;
        var userName = context.Update.CallbackQuery.From.Id;
        var channel = Database.GetChannel(channelName);
        if (channel != null) 
        {
            var user = Database.GetUser(userName);
            user.MainChannel = channelName;
            Buttons.Add(Globals.GetCommand("continue"), "/suggestionAccepted");
            return Globals.GetCommand("ChosenSuccessfully") + channelName;  
        }
        return Globals.GetCommand("SmthGoesWrong");
    }
}