using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.ObjectModel;

namespace MyTelegramBot.Types;
/// <summary>
/// Abstract Class <c>Query</c> describes a bot <c>CallbackQueries</c> handler with
/// a simplified interface, utilities and validate conditions.
/// </summary>
public abstract class Query : Listener
{
    private string[] names;
    /// <value>
    ///  Property <c>Names</c> represents the command trigger names.
    /// </value>
    public string[] Names {
        get => names;
        set {
            foreach (string name in value)
            {
                if (name.Contains(ArgumentParser.Divider))
                {
                    throw new CommandNameInvalidException(
                        "Command name must not contain command parser divider."
                    );
                }
            }
            names = value;
        }
    }
    public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.CallbackQuery)
            return false;
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        if (user == null) return false;
        if (user.RefId == null)
            return false;
        
        string messageText = context.Update.CallbackQuery.Data;
        //Console.WriteLine(messageText);
        foreach(string name in Names) {
            if (messageText.StartsWith($"{name} ") || messageText.Equals(name)) {
                return true;
            }
        }
        return false;
    }
    // public override async Task Handler(Context context, CancellationToken cancellationToken)
    // {
    //     string response = await RunAsync(context, cancellationToken);
    //     Int64 chatId = context.Update.CallbackQuery.Message.Chat.Id;

    //     if (response.Length == 0)
    //         return;

    //     Message sentMessage = await context.BotClient.SendTextMessageAsync(
    //         chatId: chatId,
    //         text: response,
    //         parseMode: Config.ParseMode
    //     );
    // }
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        var buttons = new Dictionary<string, string>(){};
        string response = Task.Run(() => Run(context, cancellationToken, out buttons)).Result;
        Int64 chatId = context.Update.CallbackQuery.Message.Chat.Id;

        List<IEnumerable<InlineKeyboardButton>> categoryList = new List<IEnumerable<InlineKeyboardButton>>();
        foreach (var category in buttons)
        {
            InlineKeyboardButton reply = InlineKeyboardButton
                .WithCallbackData(category.Key, category.Value);
            IEnumerable<InlineKeyboardButton> inlineKeyboardButton = new[] { reply };
            categoryList.Add(inlineKeyboardButton);
        }

        IEnumerable<IEnumerable<InlineKeyboardButton>> enumerableList1 = categoryList;
        InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(enumerableList1);

        Message sentMessage = await context.BotClient.SendTextMessageAsync(
            chatId: chatId,
            text: response,
            parseMode: Config.ParseMode,
            replyMarkup: inlineKeyboardMarkup
        );
    }
    /// <summary>Processes a command asynchronously.</summary>
    /// <returns>Command result string.</returns>
    public virtual async Task<string> RunAsync(Context context, CancellationToken cancellationToken) {
        return Run(context, cancellationToken);
    }
    
    /// <summary>
    ///  Creates a <c>Command</c> for the specified <c>Bot</c>.
    /// </summary>
    public Query(Bot bot) : base(bot) 
    {   
    }
}