using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using MyTelegramBot.Utils;
using Telegram.Bot.Types.ReplyMarkups;


namespace MyTelegramBot.Types ;
/// <summary>
/// Class <c>CommandNameInvalidException</c> describes an exception thrown when command name
/// for some reason is invalid.
/// </summary>
public class CommandNameInvalidException : Exception
{
    /// <summary>
    /// Creates a <c>CommandNameInvalidException</c> with default data.
    /// </summary>
    public CommandNameInvalidException() { }
    /// <summary>
    /// Creates a <c>CommandNameInvalidException</c> with certain message.
    /// </summary>
    /// <param name="message">the message that describes the error</param>
    public CommandNameInvalidException(string message) : base(message) { }
    /// <summary>
    /// Creates a <c>CommandNameInvalidException</c> with certain message 
    /// and inner exception.
    /// </summary>
    /// <param name="message">the message that describes the error</param>
    /// <param name="inner">
    /// the exception that is a cause of the current 
    /// exception or a null-reference if no inner exception is specified.
    /// </param>
    public CommandNameInvalidException(string message, Exception inner): base(message, inner) { }
}
/// <summary> 
/// Abstract Class <c>Command</c> describes a bot command handler with
/// a simplified interface, utilities and validate conditions.
/// </summary>
public abstract class Command : Listener
{
    /// <value>
    /// Property <c>ArgumentParser</c> represents the command argument parser.
    /// </value>
    
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
    /// <summary>
    ///  Creates a <c>Command</c> for the specified <c>Bot</c>.
    /// </summary>
    public Command(Bot bot) : base(bot) 
    {
    }
    /// <summary>Checks if the <c>Command</c> matches the command conditions.</summary>
    public async override Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.Message)
            return false;
        if (context.Update.Message!.Type != MessageType.Text)
            return false;
        var user = Database.GetUser(context.Update.Message.From.Id);
        if (user == null) return false;
        if (user.RefId == null)
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
    /// <summary>Handles the <c>Command</c> if it is successfully validated.</summary>
    // public override async Task Handler(Context context, CancellationToken cancellationToken)
    // {
    //     // var thread = new Thread(new ThreadStart(this.Run));
    //     string response = await RunAsync(context, cancellationToken);
    //     Int64 chatId = context.Update.Message.Chat.Id;
        
    //     if (response.Length == 0)
    //     {
    //         return;
    //     }
    //     Message sentMessage = await context.BotClient.SendTextMessageAsync(
    //         chatId: chatId,
    //         text: response,
    //         parseMode: Config.ParseMode
    //     );
    // }

    /// <summary>
    /// Handles the <c>Command</c> using <param name="buttonsList"></param>if it is successfully validated 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="buttonsList"></param>
    /// <param name="cancellationToken"></param>
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        var buttons = new Dictionary<string, string>(){};
        string response;
        if(this.HandleType == HandleType.Standard) response = Task.Run(() => RunAsync(context, cancellationToken)).Result;
        else response = Task.Run(() => Run(context, cancellationToken, out buttons)).Result;
        Int64 chatId = context.Update.Message.Chat.Id;
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
}
