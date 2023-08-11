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
        var category = Database.GetCategory(ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText).Result;
        user.Categories.Add(category.TelegramId);
        Console.WriteLine(ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText);
        user.Update();
        return base.Run(context, cancellationToken);
    }
}
