using System.Collections;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Listeners;

public class CatalogCommand : Query
{
    public CatalogCommand(Bot bot): base(bot) {
        Names = new string[]{"/catalog"};
        Buttons= new Dictionary<string, string>()
        {
            {"Новости и медиа", "/listOfSubcategories"}, //TODO: get list from db
            {"Технологии и IT", "list of subcategories"},
            {"Финансы и инвестиции", "list of subcategories"},
            {"Путешествия и туризм", "list of subcategories"},
            {"Здоровье и фитнес", "list of subcategories"},
            {"Мода и красота", "list of subcategories"},
            {"Музыка и аудио", "list of subcategories"},
            {"Спорт", "list of subcategories"},
            {"Игры и гейминг", "list of subcategories"},
            {"Искусство и дизайн", "list of subcategories"},
            {"Психология и отношения", "list of subcategories"},
            {"Образование и учеба", "list of subcategories"},
            {"Животные и природа", "list of subcategories"},
            {"Автомобили и техника", "list of subcategories"},
            {"Дом и интерьер", "list of subcategories"},
            {"Бизнес и стартапы", "list of subcategories"},
            {"Красота и уход", "list of subcategories"},
            {"Криптовалюты", "list of subcategories"},
            {"Маркетинг/PR", "list of subcategories"},
            {"Мотивация и саморазвитие", "list of subcategories"},
            {"Наука", "list of subcategories"},
            {"Недвижимость", "list of subcategories"},
            {"Религия и духовность", "list of subcategories"},
            {"Заработок", "list of subcategories"},
            {"Ставки и азартные игры", "list of subcategories"},
            {"Строительство и ремонт", "list of subcategories"},
            {"18+", "list of subcategories"},
            {"Хобби", "list of subcategories"},
            {"Юриспруденция", "list of subcategories"},
            {"Развлечения и отдых", "list of subcategories"}
        };
    }
    public override string Run(Context context, CancellationToken cancellationToken)
    {
        return "Каталог категорий";
    }
}


