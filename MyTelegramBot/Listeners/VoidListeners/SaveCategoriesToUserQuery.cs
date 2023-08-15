using MongoDatabase.ModelTG;
using MyTelegramBot.Convertors;
using MyTelegramBot.Types;
using System.Globalization;
using Serilog;

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
        User user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        Log.Information($"User: {user}");
        var category = Database.GetCategory(ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText).Result;
        Log.Information($"Category: {category}");
        var userName = user.UserName;
        var categoryName = category.Title;
        var categoryTelegramId = category.TelegramId;
        Log.Information($"Add {categoryName} with Id: {categoryTelegramId} to user {userName}.");
        if(user == null) throw new NullReferenceException("User not found in DB");
        if(category == null) throw new NullReferenceException("Category not found in DB");
        user.Categories.Add(category.TelegramId);
        Log.Information(ArgumentParser.Parse(context.Update.CallbackQuery.Data).ArgumentsText);
        user.Update();
        return "";
    }
}
