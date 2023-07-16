using MongoDatabase.ModelTG;
using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class SaveCategoriesToUserQuery : Query
{
    public SaveCategoriesToUserQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/saveCategoryToUser" };
    }

    public override string Run(Context context, CancellationToken cancellationToken)
    {
        var user = GetUserSync(context.Update.CallbackQuery.From.Id);
        var newUser = user;
        newUser.Categories.Add(context.Update.CallbackQuery.Data);
        Console.WriteLine(context.Update.CallbackQuery.Data);
        newUser.Update();
        return base.Run(context, cancellationToken);
    }
}
