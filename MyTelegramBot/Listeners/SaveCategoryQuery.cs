using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class SaveCategoryQuery : Query
{
    public SaveCategoryQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/saveCategory" };
        MessageToSend = "👌 Отлично! Теперь отправь мне Username своего канала, в таком виде: @channel" +
                        "💡 Канал обязательно должен быть открытым.";
    }
}