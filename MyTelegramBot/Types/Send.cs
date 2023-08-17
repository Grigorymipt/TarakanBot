using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace MyTelegramBot.Types;

public static class Send
{
    public static void Document(Context context,
        string FileName,
        string? pathToFile = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        pathToFile ??= Environment.GetEnvironmentVariable("pathToMaterials");
        Document(context, pathToFile + FileName, cancellationToken: cancellationToken);
    }


    public static void Document(
        Context context, 
        string fullPath, 
        CancellationToken cancellationToken = default(CancellationToken))
    {
        Task.Run(() => 
        {
            try 
            {
                ChatId chatId = context.Update.Type switch
                {
                    UpdateType.Message => context.Update.Message.Chat.Id,
                    UpdateType.CallbackQuery => context.Update.CallbackQuery.Message.Chat.Id,
                    _ => throw new NullReferenceException("Not Supported Type of update")
                };
                var fileStream = File.OpenRead(fullPath);
                InputFile file = new InputFileStream(fileStream);
                var document = context.BotClient.SendDocumentAsync(
                    chatId: chatId,
                    document: file,
                    cancellationToken: cancellationToken).Result;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
            }
            
        });
    }
    public static async void Video(Context context, string path, CancellationToken cancellationToken = default(CancellationToken))
    {
        Task.Run(() => 
        {
            try
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
            catch(Exception ex)
            {
                Log.Error(ex.Message);
            }
        });
    }
    public static async void Photo(Context context, string path, CancellationToken cancellationToken = default(CancellationToken))
    {
        Task.Run(() => 
        {
            try
            {
                new CancellationTokenSource().CancelAfter(7000);
                ChatId chatId = context.Update.Message.Chat.Id;
                var fileStream = File.OpenRead(path);
                InputFile file = new InputFileStream(fileStream);
                var document = context.BotClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: file,
                    cancellationToken: cancellationToken
                ).Result;
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
            }
        });
    }
    
}