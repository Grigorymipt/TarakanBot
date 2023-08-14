using MyTelegramBot;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

public class WalletReply : Listener
{
    public string Payload { get;set; } = "";
    public WalletReply(Bot bot) : base(bot) { 

    }
}