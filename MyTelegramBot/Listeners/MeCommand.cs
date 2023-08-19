using System.Threading;
using System.Threading.Tasks;
using MyTelegramBot.Types;
using Telegram.Bot.Types;
using MyTelegramBot.Convertors;

namespace MyTelegramBot.Listeners {
    public class MeCommand : Command, IListener {
        public MeCommand(Bot bot): base(bot) {
            Names = new string[]{"/me"};
        }
        protected override HandleParameters GetSendParameters(Context context, CancellationToken cancellationToken)
        {
            HandleParameters handleParameters = new();
            var message = context.Update.Message;
            var user = Database.GetUser(message).Result;
            handleParameters.MessageToSend = Globals.GetCommand("YourTarakans") + user.Tarakans.Count;
            return handleParameters;
        }
    }
}