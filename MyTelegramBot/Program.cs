using System;
using Telegram.Bot;
using Serilog;
using Bridge;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using SharpCompress.Common;

namespace MyTelegramBot;

public static class Globals
{
    private static Dictionary<string, string> GetDictionary()
    {
        Dictionary<string, string> responses = new();
        string[] lines = System.IO.File.ReadAllLines(Environment.GetEnvironmentVariable("pathToMaterials") + "commands.csv");  
        foreach(var line in lines)
        {
            string[] Line = line.Split(",", 2);
            if(Line.Count() != 2) throw new ExtractionException("file is invalid");
            responses.Add(Line[0], Line[1]);
        }
        return responses;
    }
    public readonly static Dictionary<string, string> responses = GetDictionary();
}
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
        Log.Information(Globals.responses.GetValueOrDefault("laststep"));
        // Log.Information("helloELKFromGrigorymipt");
        // Log.Information(Config.BotToken);
        // TelegramBotClient botClient = new TelegramBotClient(Config.BotToken);
        // Bot bot = new Bot(botClient: botClient, logger: new Logger<Bot>(new LoggerFactory()))
        // {
        //     Token = Config.BotToken,
        // };
        // Task.Run(() => WebHook.UpdateReceive(null));
        // bot.Init().Wait();
    }
}

