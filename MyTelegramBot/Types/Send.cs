using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace MyTelegramBot.Types;

public static class Send
{
    public static void Document(Context context, string path, CancellationToken cancellationToken = default(CancellationToken))
    {
        ChatId chatId = context.Update.Message.Chat.Id;
        var fileStream = File.OpenRead(path);
        InputFile file = new InputFileStream(fileStream);
        var document = context.BotClient.SendDocumentAsync(
            chatId: chatId,
            document: file,
            cancellationToken: cancellationToken
        ).Result;
    }
    public static void Video(Context context, string path, CancellationToken cancellationToken = default(CancellationToken))
    {
        ChatId chatId = context.Update.Message.Chat.Id;
        var fileStream = File.OpenRead(path);
        InputFile file = new InputFileStream(fileStream);
        var document = context.BotClient.SendVideoAsync(
            chatId: chatId,
            video: file,
            cancellationToken: cancellationToken
        ).Result;
    }
    public static void Photo(Context context, string path, CancellationToken cancellationToken = default(CancellationToken))
    {
        ChatId chatId = context.Update.Message.Chat.Id;
        var fileStream = File.OpenRead(path);
        InputFile file = new InputFileStream(fileStream);
        var document = context.BotClient.SendPhotoAsync(
            chatId: chatId,
            photo: file,
            cancellationToken: cancellationToken
        ).Result;
    }
    
}