using System.Reflection;
using MongoDatabase;
using MongoDatabase.ModelTG;
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
        Console.WriteLine("Initializing bot...");
        Log.Information("Initializing bot...");
        
        var users = new UserRepository();
        var su = new MongoDatabase.ModelTG.User()
        {
            TelegramId = 0,
            UserName = "SuperUser"
        };
        users.CreateDocument(su);

        List<string> CheckCategories = new List<string>()
        {
            "Новости и медиа" , 
            "Технологии и IT",
            "Финансы и инвестиции",
            "Путешествия и туризм",
            "Здоровье и фитнес",
            "Мода и красота",
            "Музыка и аудио",
            "Спорт",
            "Игры и гейминг",
            "Искусство и дизайн",
            "Психология и отношения",
            "Образование и учеба",
            "Животные и природа",
            "Автомобили и техника",
            "Дом и интерьер",
            "Бизнес и стартапы",
            "Красота и уход",
            "Криптовалюты",
            "Маркетинг/PR",
            "Мотивация и саморазвитие",
            "Наука",
            "Недвижимость",
            "Религия и духовность",
            "Заработок",
            "Ставки и азартные игры",
            "Строительство и ремонт",
            "18+",
            "Хобби",
            "Юриспруденция",
            "Развлечения и отдых"
        };

        var collection = new CategoryRepository();
        var i = collection.GetAllDocumentsAsync().Result.Count;
        foreach (var variableCategory in CheckCategories)
        {
            var category = await Database.GetCategory(variableCategory);
            if (category == null){
                Category newCategory = new Category()
                {
                    TelegramId = i,
                    Title = variableCategory
                };
                i+=1;
                collection.CreateDocument(newCategory);
            }
        }        
        
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
            Description = "Пользовательское меню"
        });
        listCommands.Add(new BotCommand()
        {
            Command = "watchmovies",
            Description = "Люблю смотреть фильмы"
        });
        await _botClient.SetMyCommandsAsync(
            listCommands.AsEnumerable()
            );
        
        //Login Telegram API account:
        

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
            await ChannelInfo.Login();
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
    public async Task SuccessPayment(string Id, CancellationToken cancellationToken)
    {
        var user = Database.GetUserByTrnId(Id);
        if (user == null) throw new NullReferenceException("User not found!");

    }   

    private static IEnumerable<Type> GetTypesImplementedBy<T>(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(T))))
        {
            yield return type;
        }
    }
}