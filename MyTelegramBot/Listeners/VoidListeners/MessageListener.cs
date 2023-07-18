using System.Threading;
using System.Threading.Tasks;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Listeners {
    public class MessageListener : Listener
    {
        public MessageListener(Bot bot):base(bot) {}
        public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
        {
            if (context.Update.Type != UpdateType.Message)
                return false;
            return true;
        }
        
        public override async Task Handler(Context context, CancellationToken cancellationToken)
        {
            var user = await Database.GetUserAsync(context.Update.Message.From.Id);
            if (user != null)
            {
                user.Messages++;
                user.Update(); 
            }
        }

        public override async Task Handler(Context context, Dictionary<string, string> buttonList,
            CancellationToken cancellationToken)
        {
            Handler(context, cancellationToken);
        }
    }
}