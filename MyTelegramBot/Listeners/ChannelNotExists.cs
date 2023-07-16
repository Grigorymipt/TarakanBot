using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class ChannelNotExists : Query
{
    public ChannelNotExists(Bot bot) : base(bot)
    {
        Names = new[] { "/channelExists" };
        MessageToSend = "🤯  Такого канала не существует.Проверь пожалуйста Username и отправь сообщение еще раз!";
    }
}