using System.Collections;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Listeners;

public class CatalogCommand : Command
{
    public CatalogCommand(Bot bot): base(bot) {
        Names = new string[]{"/catalog"};
    }
    public override string Run(Context context, CancellationToken cancellationToken)
    {
        return "Каталог категорий";
    }

    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        string response = await RunAsync(context, cancellationToken);
        Int64 chatId = context.Update.Message.Chat.Id;
        List<string> categories = new List<string>()
        {
            "Новости и медиа", //TODO: get list from db
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
        List<IEnumerable<InlineKeyboardButton>> categoryList = new List<IEnumerable<InlineKeyboardButton>>();
        foreach (var category in categories)
        {
            InlineKeyboardButton reply = InlineKeyboardButton.WithCallbackData(category, "/NewsAndMedia");
            Console.WriteLine(reply);
            IEnumerable<InlineKeyboardButton> inlineKeyboardButton = new[] { reply };
            categoryList.Add(inlineKeyboardButton);
        }

        IEnumerable<IEnumerable<InlineKeyboardButton>> enumerableList1 = categoryList;
        InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(enumerableList1);
        Message sentMessage = await context.BotClient.SendTextMessageAsync(
            chatId: chatId,
            text: response,
            parseMode: Config.ParseMode,
            replyMarkup: inlineKeyboardMarkup
        );
    }
}


