using MyTelegramBot.Types;
using Telegram.Bot.Types;

namespace MyTelegramBot;

public class ChoseCategoryQuery : Query
{
    public ChoseCategoryQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/addChannel" };
        Buttons= new Dictionary<string, string>()
        {
            {"Новости и медиа", "/saveCategory"}, //TODO: get list from db
            {"Технологии и IT", "/saveCategory"},
            {"Финансы и инвестиции", "/saveCategory"},
            {"Путешествия и туризм", "/saveCategory"},
            {"Здоровье и фитнес", "/saveCategory"},
            {"Мода и красота", "/saveCategory"},
            {"Музыка и аудио", "/saveCategory"},
            {"Спорт", "/saveCategory"},
            {"Игры и гейминг", "/saveCategory"},
            {"Искусство и дизайн", "/saveCategory"},
            {"Психология и отношения", "/saveCategory"},
            {"Образование и учеба", "/saveCategory"},
            {"Животные и природа", "/saveCategory"},
            {"Автомобили и техника", "/saveCategory"},
            {"Дом и интерьер", "/saveCategory"},
            {"Бизнес и стартапы", "/saveCategory"},
            {"Красота и уход", "/saveCategory"},
            {"Криптовалюты", "/saveCategory"},
            {"Маркетинг/PR", "/saveCategory"},
            {"Мотивация и саморазвитие", "/saveCategory"},
            {"Наука", "/saveCategory"},
            {"Недвижимость", "/saveCategory"},
            {"Религия и духовность", "/saveCategory"},
            {"Заработок", "/saveCategory"},
            {"Ставки и азартные игры", "/saveCategory"},
            {"Строительство и ремонт", "/saveCategory"},
            {"18+", "/saveCategory"},
            {"Хобби", "/saveCategory"},
            {"Юриспруденция", "/saveCategory"},
            {"Развлечения и отдых", "/saveCategory"}
        };
        MessageToSend = "Выбери одну из категорий которая максимально подходит твоему каналу. ✅ ";
    }
}