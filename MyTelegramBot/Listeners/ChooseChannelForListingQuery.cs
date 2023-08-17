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
            Buttons.Add(Globals.responses.GetValueOrDefault("continue"), "/suggestionAccepted");
            return Globals.responses.GetValueOrDefault("ChosenSuccessfully") + channelName;  
        }
        return Globals.responses.GetValueOrDefault("SmthGoesWrong");
    }
}