using System;
using Telegram.Bot;

namespace MyTelegramBot
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine(Config.BotToken);
            TelegramBotClient botClient = new TelegramBotClient(Config.BotToken);
            Bot bot = new Bot(botClient: botClient, logger: new Logger<Bot>(new LoggerFactory()))
            {
                Token = Config.BotToken,
            };
            Task.Run(() => WebHook.UpdateReceive(null));
            bot.Init().Wait();
        }
    }
}