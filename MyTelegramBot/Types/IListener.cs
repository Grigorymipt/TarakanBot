using MyTelegramBot.Utils;

namespace MyTelegramBot.Types;

public interface IListener
{
    public CommandParser ArgumentParser { get; set; }
}