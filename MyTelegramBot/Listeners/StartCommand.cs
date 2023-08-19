using System.Threading;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Serilog;
using User = MyTelegramBot.Types.User;

namespace MyTelegramBot.Listeners ;
public class StartCommand : Command, IListener{
    public StartCommand(Bot bot): base(bot) {
        Names = new string[]{"/start"};
        HandleType = HandleType.Standard;
    }
    protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
    {
        HandleParameters handleParameters = new();
        handleParameters.MessageToSend = Globals.GetCommand("Start");
        handleParameters.buttons.Add(Globals.GetCommand("StatsButton"), "/catalog");
        handleParameters.buttons.Add(Globals.GetCommand("AddTarakanButton"), "/addTarakan");
        new User(context.Update.Message.From.Id, context.Update.Message.From.Username).Create();
        return handleParameters;
    }
}
