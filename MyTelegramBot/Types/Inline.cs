using MongoDatabase.ModelTG;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Types;
/// <summary>
/// Abstract Class <c>InlineQuery</c> sends Query, <c>Message</c> answer expected
/// </summary>
public abstract class InlineQuery : Query
{
    public string MessageLabel { get; set; }
    public InlineQuery(Bot bot) : base(bot){}

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>();
        // Console.WriteLine(context.Update.CallbackQuery.From.Id);
        var user = Database.GetUser(context.Update.CallbackQuery.From.Id);
        Console.WriteLine(MessageLabel);
        user.LastMessage = MessageLabel;
        user.Update();
        return MessageToSend[0];
    }
    
}
/// <summary>
/// Abstract Class <c>InlineReply</c> catch users reply on according <c>InlineQuery</c>
/// </summary>
public abstract class InlineReply : Command
{
    public string MessageLabel { get; set; }

    public InlineReply(Bot bot) : base(bot)
    {
    }
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        Console.WriteLine(MessageLabel);
        await base.Handler(context, cancellationToken);
        var user = Database.GetUser(context.Update.Message.From.Id);
        user.LastMessage = null;
        user.Update();
    }
    public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.Message)
            return false;
        var user = Database.GetUser(context.Update.Message.From.Id);
        if (user == null) return false;
        if (user.RefId == null)
            return false;
        if (user.LastMessage == MessageLabel)
            return true;
        return false;
    }
}
