using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDatabase;
using MongoDatabase.ModelTG;
using MyTelegramBot.Listeners;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using User = Telegram.Bot.Types.User;

namespace MyTelegramBot {
    public class Bot {
        public List<Listener> Listeners { get; set; }
        public User Me { get; set; }
        public string Token { get; set; }
        public Bot()
        {
            Listeners = new List<Listener> {
                new StartCommand(this),
                new HelpCommand(this),
                new MeCommand(this),
                new EchoCommand(this),
                new MessageListener(this),
                new PromoCommand(this),
                new CatalogCommand(this),
                new GetAdressImAdminQuery(this),
                new NewsAndMediaQuery(this),
                new ChoseCategoryQuery(this),
                new SaveCategoryQuery(this),
                new AlmostOnTargetQuery(this),
                new SuggestionAcceptedQuery(this),
                new PayForListingQuery(this),
                new PaymentProcessingQuery(this),
                // TODO: Put more commands and other listeners.
            };
        }
        
        public async Task Init() 
        {
            Console.WriteLine("Initializing bot...");
            
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
            foreach (var variableCategory in CheckCategories)
            {
                var category = collection.GetDocument(variableCategory);
                if (category == null)
                {
                    category = new Category(){Title = variableCategory, Id = new Guid()};
                    collection.CreateDocument(category); // TODO: Use Mytelegram API
                }
            }
            
            TelegramBotClient botClient = new TelegramBotClient(Token);
            using CancellationTokenSource cts = new CancellationTokenSource();
            ReceiverOptions receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = {},
            };

            Console.WriteLine("Starting bot...");
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
            );

            Me = await botClient.GetMeAsync();
            
            Console.WriteLine($"Start listening for @{Me.Username}");
            Console.Read();

            cts.Cancel();
        }
        public Task EmitUpdate(Context context, CancellationToken cancellationToken)
        {
            return HandleUpdateAsync(context.BotClient, context.Update, cancellationToken);
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Context context = new Context(update, botClient);
            foreach (Listener listener in Listeners) {
                if (await listener.Validate(context, cancellationToken))
                {
                    if(listener.HandleType == HandleType.ButtonList)
                        await listener.Handler(context, listener.Buttons, cancellationToken);
                    else
                        await listener.Handler(context, cancellationToken);
                }
            }
        }
        async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString(),
            };
            int ErrorCode = -1;
            switch(exception)
            {
                case ApiRequestException apiRequestException:
                    ErrorCode = apiRequestException.ErrorCode;
                    break;
            };
            if (ErrorCode != 40399)
            {
                Console.WriteLine(ErrorMessage);
            }
        }
    }
}