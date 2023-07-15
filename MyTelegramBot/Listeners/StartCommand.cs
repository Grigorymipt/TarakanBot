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
            Buttons= new Dictionary<string, string>()
            {
                {"Я адмэн", "/admin"},
                {"Каталог", "/catalog"}
            };
        }
        public override string Run(Context context, CancellationToken cancellationToken)
        {
            User user = GetUserSync(context.Update.Message.From.Id); // TODO: Reduce DB calls
            if (user == null) user = CreateUser(context.Update.Message);
            else if (user.RefId == null) user = UpdateUser(context.Update.Message);
            if (user.RefId != null) return "С помощью #UserHub ты сможешь быстро и удобно находить лучшие телеграмм " +
                                           "каналы на любую интересную тебе тему. Я что-то вроде поисковика в телеграмм, " +
                                           "где все каналы разбиты по категориям и рейтингу. " +
                                           "Жми 'каталог' и я покажу тебе как здесь все устроено. " + 
                "Если же ты владелец телеграм-канала, то жми 'Я админ' и добавляй свой канал в каталог #UserHub. " +
                "Это позволить тебе получать подписчиков, действительно заинтересованных в твоем контенте.";
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