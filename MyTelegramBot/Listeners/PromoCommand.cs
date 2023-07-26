using System.Collections;
using System.Runtime.InteropServices;
using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using Message = Telegram.Bot.Types.Message;

namespace MyTelegramBot.Listeners;

public class PromoCommand : Query, IListener
{
    public PromoCommand(Bot bot) : base(bot)
    {
        Names = new string[] { "/promo" };
        Buttons = new Dictionary<string, string>()
        {
            { "PDF", "/pdf" },
            { "Креативы для Телеграм канала", "/creativesForChannel" },
            { "Видео", "/videos" },
            { "Stories фото", "/storiesPhoto" },
            { "Stories видео", "/storiesVideo" },
        };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return "Промо материалы";
    }
}

public class PDFQuery : Query, IListener
{
    public PDFQuery(Bot bot): base(bot) {
        Names = new string[]{"/pdf"};
    }
    
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Send.Document(context, "conspect.pdf", null, cancellationToken);
        return "Зачем вам этот PDF, когда надежнее пользоваться счетом древних РУСов";
    }
}

public class CreativesQuery : Query, IListener
{
    public CreativesQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/creativesForChannel" };
    }
}
public class StoriesPhotoQuery : Query, IListener
{
    public StoriesPhotoQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/storiesPhoto" };
    }
}
public class StoriesVideoQuery : Query, IListener
{
    public StoriesVideoQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/storiesVideo" };
    }
}
public class VideosQuery : Query, IListener
{
    public VideosQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/videos" };
    }
}