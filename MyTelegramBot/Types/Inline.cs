using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Types;

public class Inline : Listener
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
        if (context.Update.Type != UpdateType.Message)
            return false;
        var user = GetUserSync(context.Update.Message.From.Id);
        if (user == null) return false;
        // if (user.RefId == null)
        //     return false;
        //
        string messageText = context.Update.Message.Text;
        // Console.WriteLine(messageText);
        return false;
    }
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        string response = await RunAsync(context, cancellationToken);
        Int64 chatId = context.Update.Message.Chat.Id;
        // Console.WriteLine(context.Update.Message.Text);
        if (response.Length == 0)
        {
            return;
        }
        Message sentMessage = await context.BotClient.SendTextMessageAsync(
            chatId: chatId,
            text: response,
            parseMode: Config.ParseMode
        );
    }
    public override async Task Handler(Context context, Dictionary<string, string> buttonsList, CancellationToken cancellationToken)
    {
        string response = Run(context, cancellationToken);
        Int64 chatId = context.Update.CallbackQuery.Message.Chat.Id;
        List<IEnumerable<InlineKeyboardButton>> categoryList = new List<IEnumerable<InlineKeyboardButton>>();
        foreach (var category in buttonsList)
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
    public Inline(Bot bot) : base(bot) 
    {
    }
}