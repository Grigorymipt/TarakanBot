using System.Threading;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = MongoDatabase.ModelTG.User;

namespace MyTelegramBot.Listeners {
    public class StartCommand : Command {
        public StartCommand(Bot bot): base(bot) {
            Names = new string[]{"/start", "/starting", "!start"};
        }
        public async override Task<string> RunAsync(Context context, CancellationToken cancellationToken)
        {
            User user = await GetUser(context.Update.Message); // TODO: Reduce DB calls
            if (user == null) user = CreateUser(context.Update.Message);
            else if (user.RefId == null) user = UpdateUser(context.Update.Message);
            if (user.RefId != null) return $"Welcome! Press /help to see my functions";
            return "Этим ботом можно пользоваться, только перейдя в него " +
                   "по реферальной ссылке от пользователя, который уже имеет доступ к боту."; 
            // TODO: add collection(enum) with answers for each request(pos/neg/null)
        }
        public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
        {
            if (context.Update.Type != UpdateType.Message)
                return false;
            if (context.Update.Message!.Type != MessageType.Text)
                return false;
            string messageText = context.Update.Message.Text.Replace($"@{Bot.Me.Username}","");

            foreach(string name in Names) {
                if (messageText.StartsWith($"{name} ") || messageText.Equals(name)) {
                    return true;
                }
            }
            return false;
        }
    }
}