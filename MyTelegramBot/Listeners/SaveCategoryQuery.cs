using MyTelegramBot.Types;
using Telegram.Bot;

namespace MyTelegramBot.Listeners;

public class SaveCategoryQuery : Query // TODO: doesnt work at all
{
    public SaveCategoryQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/saveCategory" };
        MessageToSend = "👌 Отлично! Теперь отправь мне Username своего канала, в таком виде: @channel" +
                        "💡 Канал обязательно должен быть открытым.";
    }
    public override async Task<string> RunAsync(Context context, CancellationToken cancellationToken)
    {
        // Console.WriteLine(context.Update.CallbackQuery.From.Id);
        var user = GetUserSync(context.Update.CallbackQuery.From.Id);
        user.LastMessage = "GetAddressInline";
        user.Update();
        return MessageToSend;
    }
}