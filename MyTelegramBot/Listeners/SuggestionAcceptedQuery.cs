using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class SuggestionAcceptedQuery : Query
{
    public SuggestionAcceptedQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/suggestionAccepted" };
        List<string> creatives = new List<string>();
        creatives.Add("Креатив 1");
        creatives.Add("Креатив 2");
        MessageToSend = "👍 Правильный выбор! Сделай репост любого поста из предложенных ниже, подача которого " +
                        "наиболе подходит для твоей аудитории, и твой канал автоматически появится в каталоге #UserHub";
        foreach (var creative in creatives)
        {
            MessageToSend += "\n" + creative;
        }
    }
}