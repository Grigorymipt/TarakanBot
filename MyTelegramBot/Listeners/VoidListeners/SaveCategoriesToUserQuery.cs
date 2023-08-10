using MongoDatabase.ModelTG;
using MyTelegramBot.Convertors;
using MyTelegramBot.Types;
using System.Globalization;

namespace MyTelegramBot.Listeners;

public class SaveCategoriesToUserQuery : Query, IListener
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
        long id;
        long.TryParse(ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText, out id);
        Database.GetCategoryAsync(id);
        newUser.Categories.Add(id);
        Console.WriteLine(ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText);
        newUser.Update();
        return base.Run(context, cancellationToken);
    }
}
