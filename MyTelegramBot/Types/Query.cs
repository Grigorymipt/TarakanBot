using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Types;

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
        if (context.Update.Type == UpdateType.CallbackQuery)
        {
            return true;
        }
        return false;
    }
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        string response = await RunAsync(context, cancellationToken);
        Int64 chatId = context.Update.CallbackQuery.Message.Chat.Id;
            
        if (response.Length == 0)
        {
            return;
        }
        Message sentMessage = await context.BotClient.SendTextMessageAsync(
            chatId: chatId,
            text: response,
            parseMode: Config.ParseMode
            // replyToMessageId: context.Update.Message.MessageId
        );
    }
    /// <summary>Processes a command synchronously.</summary>
    /// <returns>Command result string.</returns>
    public virtual string Run(Context context, CancellationToken cancellationToken) {
        return "This command is under development and not currently available.";
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