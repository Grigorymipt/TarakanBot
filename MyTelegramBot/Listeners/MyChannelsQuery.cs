using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class MyChannelsQuery : Query, IListener
{
    public MyChannelsQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/myChannels" };
        MessageToSend = "🤝 Мои рукопожатия";
        Buttons = new Dictionary<string, string>()
        {
            {"Добавить канал", "/addChannel"},
            {"Выбрать канал", "/chooseChannel"},
        };
    }
}