using System.Threading;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = MongoDatabase.ModelTG.User;

namespace MyTelegramBot.Listeners ;
public class StartCommand : Command, IListener{
    public StartCommand(Bot bot): base(bot) {
        Names = new string[]{"/start", "/starting", "!start"};
        HandleType = HandleType.Standard;
    }
    
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "2.2", cancellationToken);
        Buttons = new Dictionary<string, string>();
        Console.WriteLine(context.Update.Message.From.Id);
        User user = Database.GetUser(context.Update.Message.From.Id); // TODO: Reduce DB calls
        
        if (user == null) user = Database.CreateUser(context.Update.Message);
        
        else if (user.RefId == null) user = Database.UpdateUser(context.Update.Message);
        
        if (user.RefId != null) 
        {
            Buttons.Add("Я адмэн", "/admin");
            Buttons.Add("Каталог", "/catalog");
            return "С помощью #UserHub ты сможешь быстро и удобно находить лучшие телеграмм " +
                                       "каналы на любую интересную тебе тему. Я что-то вроде поисковика в телеграмм, " +
                                       "где все каналы разбиты по категориям и рейтингу. " +
                                       "Жми 'каталог' и я покажу тебе как здесь все устроено. " + 
            "Если же ты владелец телеграм-канала, то жми 'Я админ' и добавляй свой канал в каталог #UserHub. " +
            "Это позволить тебе получать подписчиков, действительно заинтересованных в твоем контенте.";
        }
        return "Этим ботом можно пользоваться, только перейдя в него " +
               "по реферальной ссылке от пользователя, который уже имеет доступ к боту."; 
    }

    public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.Message)
            return false;
        if (context.Update.Message!.Type != MessageType.Text)
            return false;
        string? inputText = context.Update.Message.Text;
        if (inputText == null) return false;
        string messageText = inputText.Replace($"@{Bot?.Me?.Username?.ToString()}","");

        foreach(string name in Names) {
            if (messageText.StartsWith($"{name} ") || messageText.Equals(name)) {
                return true;
            }
        }
        return false;
    }
}
