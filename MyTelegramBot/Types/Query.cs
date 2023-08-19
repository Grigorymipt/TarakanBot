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
        
        string messageText = context.Update.CallbackQuery.Data;
        //Console.WriteLine(messageText);
        if(Names == null) throw new ArgumentNullException("Listener " + this.GetType().ToString() + " havs no Names");
        foreach(string name in Names) {
            if (messageText.StartsWith($"{name} ") || messageText.Equals(name)) {
                return true;
            }
        }
        return false;
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