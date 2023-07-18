using System.Collections;
using System.Runtime.InteropServices;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using Message = Telegram.Bot.Types.Message;

namespace MyTelegramBot.Listeners;

public class PromoCommand : Command
{
    public PromoCommand(Bot bot): base(bot) {
        Names = new string[]{"/promo", "!promo"};
    }

    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        string response = Run(context, cancellationToken);
        Int64 chatId = context.Update.Message.Chat.Id;
       
        // TODO : factory for buttons
        //TODO : remove commented 
        // ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] 
        // {
        //     new KeyboardButton[] { "PDF" },
        //     new KeyboardButton[] { "Креативы для Телеграм канала" },
        //     new KeyboardButton[] { "Видео" },
        //     new KeyboardButton[] { "Stories фото" },
        //     new KeyboardButton[] { "Stories видео" }
        //     
        // })
        // {
        //     ResizeKeyboard = true
        // };

        InlineKeyboardButton reply = InlineKeyboardButton.WithUrl("Pdf", "google.com");
        IEnumerable<InlineKeyboardButton> list = new []{reply};
        InlineKeyboardMarkup  inlineKeyboardMarkup = new InlineKeyboardMarkup(list);
        Message sentMessage = await context.BotClient.SendTextMessageAsync(
            chatId: chatId,
            text: response,
            parseMode: Config.ParseMode,
            replyMarkup: inlineKeyboardMarkup,
            replyToMessageId: context.Update.Message.MessageId
        );
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return "Промо материалы";
    }
}