using System.Collections;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Listeners;

public class WhereQuery : Query, IListener
{
    public WhereQuery(Bot bot): base(bot) {
        Names = new string[]{"/where"};
    }
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters handleParameters = new();
        handleParameters.MessageToSend = Globals.GetCommand("When");
        var argument = ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText;
        handleParameters.buttons.Add(Globals.GetCommand("Remember"), $"/when {argument} yes");
        handleParameters.buttons.Add(Globals.GetCommand("NotRemember"), $"/when {argument} no");
        return handleParameters;
    }
}