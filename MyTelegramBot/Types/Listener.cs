using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading;
using MongoDatabase;
using MongoDatabase.ModelTG;
using MyTelegramBot.Convertors;
using Document = MongoDatabase.ModelTG.Document;
using User = MongoDatabase.ModelTG.User;
using MyTelegramBot.Types;
using MyTelegramBot.Utils;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.ObjectModel;


namespace MyTelegramBot.Types ;
/// <summary>Abstract Class <c>Listener</c> describes a bot event handler with 
/// utilities and validate conditions. </summary>
public abstract class Listener
{
    /// <value>
    /// Property <c>Bot</c> represents a <c>Bot</c> instance with which <c>Listener</c> is related.
    /// </value>
    public Bot Bot { get; set; }
    

    public HandleType HandleType { get; set; } = HandleType.Standard;
    private ReadOnlyDictionary<string, string> buttons;
    public ReadOnlyDictionary<string, string> Buttons
    {
        get => buttons;
        init
        {
            buttons = value;
            HandleType = HandleType.ButtonList;
        }
    }

    private string filePath;
    public bool fileToSend { get; set; } = false;
    protected string[] MessageToSend { get; init; } = {"This command is under development and not currently available."};

    private bool withoutMessage = false;
    public bool WithoutMessage
    {
        get => withoutMessage;
        init
        {
            if (value)
                MessageToSend[0] = "";
            withoutMessage = value;
        }
    }

    private string callbackQueryToSend;

    public string CallbackQueryToSend
    {
        get => callbackQueryToSend;
        set
        {
            WithCallbackQuery = true;
            callbackQueryToSend = value;
        }
    }

    public bool WithCallbackQuery { get; set; } = false;

    public string FilePath
    {
        get => filePath;
        set
        {
            filePath = value;
            fileToSend = true;
        }
    }

    /// <summary>
    ///  Creates a <c>Listener</c> for the specified <c>Bot</c>.
    /// </summary>
    public Listener(Bot bot)
    {
        Bot = bot;
        ArgumentParser = new CommandParser();
    }
    public CommandParser ArgumentParser { get; set; }
    /// <summary>Checks if the <c>Update</c> matches the listener condition.</summary>
    public abstract Task<bool> Validate(Context context, CancellationToken cancellationToken);
    /// <summary>Handles the <c>Update</c> if it is successfully validated.</summary>
    
    public virtual async Task Handler(Context context, CancellationToken cancellationToken)
    {
        var buttons = new Dictionary<string, string>(){};
        string response = Task.Run(() => Run(context, cancellationToken, out buttons)).Result;
        Int64 chatId = ChatId(context);
        
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
        try
        {
            Message sentMessage = await context.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: response,
                parseMode: Config.ParseMode,
                replyMarkup: inlineKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // public abstract Task Handler(Context context, Dictionary<string, string> buttonsList,
    //     CancellationToken cancellationToken);
    
    /// <summary>Processes a command synchronously.</summary>
    /// <returns>Command result string.</returns>
    protected virtual string Run(Context context, CancellationToken cancellationToken) {
        var buttons = new Dictionary<string, string>();
        return Run(context, cancellationToken, out buttons);
    }
    protected virtual string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons) {
        buttons = new Dictionary<string, string>();
        return Run(context, cancellationToken);
    }
    /// <summary>Processes a command asynchronously.</summary>
    /// <returns>Command result string.</returns>
    protected virtual async Task<string> RunAsync(Context context, CancellationToken cancellationToken) {
        return Run(context, cancellationToken);
    }
    protected long ChatId(Context context) {
        Update update = context.Update;
        return this switch{
            Command command => update.Message.From.Id,
            Query query => update.CallbackQuery.From.Id,
            PayloadReply payloadReply => update.PreCheckoutQuery.From.Id,
            _ => 0
        };
    }
}
