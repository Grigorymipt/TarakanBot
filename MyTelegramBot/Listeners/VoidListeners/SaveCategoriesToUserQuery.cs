using MongoDatabase.ModelTG;
using MyTelegramBot.Convertors;
using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class SaveCategoriesToUserQuery : Query
{
    public SaveCategoriesToUserQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/saveCategoryToUser" };
        WithoutMessage = true;
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        var newUser = user;
        newUser.Categories.Add(new Guid(ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText));
        Console.WriteLine(ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText);
        newUser.Update();
        return base.Run(context, cancellationToken);
    }
}
