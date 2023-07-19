using System.Threading;
using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners {
    public class HelpCommand : Command, IListener {
        public HelpCommand(Bot bot): base(bot) {
            Names = new string[]{"/help", "!help"};
        }

        protected override string Run(Context context, CancellationToken cancellationToken)
        {
            return "<b>MyTestBot commands</b> \n\n" +
                   "/start - starts bot \n" +
                   "/help - opens this message \n" +
                   "/me - user profile \n" +
                   "/catalog - каталог категорий \n" +
                   "/promo - user profile \n\n" +
                   "Bot calculates message number for each user in each chat.";
        }
    }
}