using System.Reflection;
using MyTelegramBot.Types;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TL;
using User = Telegram.Bot.Types.User;
using Update = Telegram.Bot.Types.Update;
using BotCommand = Telegram.Bot.Types.BotCommand;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Payments;

namespace MyTelegramBot ;
public class Bot {
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<Bot> _logger;

    public List<Listener> Listeners { get; set; }
    public User Me { get; set; }
    public string Token { get; set; }
    public Bot(ITelegramBotClient botClient, ILogger<Bot> logger)
    {
        _botClient = botClient;
        _logger = logger;
        Listeners = new List<Listener>() { };
        foreach(var type in GetTypesImplementedBy<IListener>(Assembly.GetExecutingAssembly()))
        {
            Listener instance = (Listener)Activator.CreateInstance(type, this);
            Listeners.Add(instance);
        }
        Console.WriteLine("Total Listeners: " + Listeners.Count);
        Log.Information("Total Listeners: " + Listeners.Count);
    }
    
    public async Task Init() 
    {
        Log.Information("Initializing bot...");
        
        new Types.User(0, "SuperUser").Create();
        
        using CancellationTokenSource cts = new CancellationTokenSource();
        //TODO: remove hardcode
        
        
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = {},
        };
         
        // TODO: menu init encapsulate
        var listCommands = new List<BotCommand>();
        listCommands.Add(new BotCommand()
        {
            Command = "menu",
            Description = "Menu"
        });
        await _botClient.SetMyCommandsAsync(
            listCommands.AsEnumerable()
            );        

        Console.WriteLine("Starting bot...");

        if(Environment.GetEnvironmentVariable("localy") == "true")
        {
            await _botClient.DeleteWebhookAsync();
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
            );
            Console.WriteLine("Long Polling configured!");
        }

        else
        {
            await _botClient.SetWebhookAsync(
            url: Environment.GetEnvironmentVariable("WebHookURL"),
            maxConnections: default,
            allowedUpdates: default,
            dropPendingUpdates: default,
            secretToken: null,
            cancellationToken: cts.Token
            );
            // await ChannelInfo.Login(null);
            Console.WriteLine("Webhook configured!");
        }
        Me = await _botClient.GetMeAsync();
        
        Console.WriteLine($"Start listening for @{Me.Username}");
        Console.Read();

        cts.Cancel();
    }
    public Task EmitUpdate(Context context, CancellationToken cancellationToken)
    {
        return HandleUpdateAsync(context.BotClient, context.Update, cancellationToken);
    }
   //TODO: unused para botClient
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if(botClient == null) botClient = _botClient; 
        Console.WriteLine("Update Received!");
        Context context = new Context(update, botClient);
        Console.WriteLine("context updated");
        foreach (Listener listener in Listeners) {
            if (await listener.Validate(context, cancellationToken))
            {
                Console.WriteLine("Start Handling With" + listener.ToString());
                await listener.Handler(context, cancellationToken);
            }
        }
        Console.WriteLine("Update Handled");
    }
    public async Task<string> HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString(),
        };
        return ErrorMessage;
    }
    private static IEnumerable<Type> GetTypesImplementedBy<T>(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(T))))
        {
            yield return type;
        }
    }
}