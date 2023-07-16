using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Types;

public class InlineQuery : Query
{
    public string MessageLabel { get; set; }
    public InlineQuery(Bot bot) : base(bot){}
    public override async Task<string> RunAsync(Context context, CancellationToken cancellationToken)
    {
        // Console.WriteLine(context.Update.CallbackQuery.From.Id);
        var user = GetUserSync(context.Update.CallbackQuery.From.Id);
        user.LastMessage = MessageLabel;
        user.Update();
        return MessageToSend;
    }
    
}
public class InlineReply : Command
{
    public string MessageLabel { get; set; }

    public InlineReply(Bot bot) : base(bot)
    {
    }
    public override string Run(Context context, CancellationToken cancellationToken)
    {
        Console.WriteLine(context.Update.Message.From.Id);
        var user = GetUserSync(context.Update.Message.From.Id);
        Console.WriteLine(context.Update.Message.Text);
        string newChannel = context.Update.Message.Text;
        var newUser = user;
        newUser.Channels.Add(newChannel); // FIXME: very strange behavior
        newUser.LastMessage = null;
        newUser.Update();
        return MessageToSend;
    }

    public override async Task<bool> Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.Message)
            return false;
        var user = GetUserSync(context.Update.Message.From.Id);
        if (user == null) return false;
        if (user.RefId == null)
            return false;
        if (user.LastMessage == MessageLabel)
            return true;
        return false;
    }
}
