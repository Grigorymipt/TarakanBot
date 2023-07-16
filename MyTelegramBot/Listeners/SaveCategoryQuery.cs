using MyTelegramBot.Types;
using Telegram.Bot;

namespace MyTelegramBot.Listeners;

public class SaveCategoryQuery : InlineQuery 
{
    public SaveCategoryQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/saveCategory" }; //TODO: ADD db logics
        MessageToSend = "👌 Отлично! Теперь отправь мне Username своего канала, в таком виде: @channel" +
                        "💡 Канал обязательно должен быть открытым.";
        MessageLabel = "GetAddressInline";
    }
}