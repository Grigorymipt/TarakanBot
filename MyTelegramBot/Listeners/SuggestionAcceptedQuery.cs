using MyTelegramBot.Types;

namespace MyTelegramBot.Listeners;

public class SuggestionAcceptedQuery : Query, IListener
{
    public SuggestionAcceptedQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/suggestionAccepted" };
        List<string> creatives = new();

        //add logics to get creatives link
        creatives.Add("Креатив 1");
        creatives.Add("https://t.me/vokyj/4");
        MessageToSend = new string[] {"👍 Правильный выбор! Сделай репост любого поста из предложенных ниже, подача которого " +
                        "наиболе подходит для твоей аудитории, и твой канал автоматически появится в каталоге #UserHub"
                        };
        foreach (var creative in creatives)
        {
            MessageToSend[0] += "\n" + creative;
        }
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return MessageToSend[0];
    }
}