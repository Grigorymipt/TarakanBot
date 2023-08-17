using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class CorrectTestAnswer : Query, IListener
{
    public CorrectTestAnswer(Bot bot) : base(bot)
    {
        Names = new[] { "/hundredMillions" };
        MessageToSend = new[] {Globals.GetCommand("fivecats")};

                        
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        var categories = Database.GetAllCategories().Result;
        buttons = new Dictionary<string, string>();
        foreach (var category in categories)
        {
            buttons.Add(category.Title, "/saveCategoryToUser " + category.TelegramId);
        }
        buttons.Add(Globals.GetCommand("continue"), "/continueTo");
        return MessageToSend[0];
    }
}
