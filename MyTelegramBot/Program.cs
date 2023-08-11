using System;
using Telegram.Bot;
using Serilog;
using Bridge;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;


namespace MyTelegramBot
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var elkConfiguration = EnvironmentBinder.Bind<ELKConfiguration>();
            var logger =
                LoggingConfigurator.ElasticLogger("userhub",
                    elkConfiguration.Username, //todo: завести новое api 
                    elkConfiguration.Password,
                    elkConfiguration.Host);
            Log.Logger = logger;
            logger.Information("helloELKFromGrigorymipt");
            Log.Information("helloELKFromGrigorymipt");
            // Console.WriteLine(Config.BotToken);
            // TelegramBotClient botClient = new TelegramBotClient(Config.BotToken);
            // Bot bot = new Bot(botClient: botClient, logger: new Logger<Bot>(new LoggerFactory()))
            // {
            //     Token = Config.BotToken,
            // };
            // Task.Run(() => WebHook.UpdateReceive(null));
            // bot.Init().Wait();
        }
    }
}