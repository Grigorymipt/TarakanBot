using System.Threading;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = MongoDatabase.ModelTG.User;
using Serilog;

namespace MyTelegramBot.Listeners ;
public class StartCommand : Command, IListener{
    public StartCommand(Bot bot): base(bot) {
        Names = new string[]{"/start", "/starting", "!start"};
        HandleType = HandleType.Standard;
    }
    
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "admin.jpg", cancellationToken);

        Buttons = new Dictionary<string, string>();
        Console.WriteLine(context.Update.Message.From.Id);
        Log.Information(context.Update.Message.From.Id.ToString());
        User user = Database.GetUser(context.Update.Message.From.Id); // TODO: Reduce DB calls

        if (user == null) user = Database.CreateUser(context.Update.Message);

        else if (user.RefId == null) user = Database.UpdateUser(context.Update.Message);


        
        if (user.RefId != null) 
        {
            Buttons.Add(Globals.GetCommand("ImAdmin"), "/admin");
            Buttons.Add(Globals.GetCommand("Catalog"), "/catalog");
            return Globals.GetCommand("StartRef");
        }
        return Globals.GetCommand("StartNoRef");
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
