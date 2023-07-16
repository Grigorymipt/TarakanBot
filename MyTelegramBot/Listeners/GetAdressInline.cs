using MyTelegramBot.Types;
using Telegram.Bot.Types;
using User = MongoDatabase.ModelTG.User;

namespace MyTelegramBot.Listeners;

public class GetAdressInline : Inline
{
    public GetAdressInline(Bot bot) : base(bot)
    {
        MessageToSend = "well";
        
    }

    
}