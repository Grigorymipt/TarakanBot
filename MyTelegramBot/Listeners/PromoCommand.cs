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
        
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>()
        {
            { Globals.GetCommand("PromoPDF"), "/pdf" },
            { Globals.GetCommand("Creatives"), "/creativesForChannel" },
            { Globals.GetCommand("PromoVideos"), "/videos" },
            { Globals.GetCommand("StoriesPhoto"), "/storiesPhoto" },
            { Globals.GetCommand("StoriesVideo"), "/storiesVideo" },
        };
        return Globals.GetCommand("Promo");
    }
}

public class PDFQuery : Query, IListener
{
    public PDFQuery(Bot bot): base(bot) {
        Names = new string[]{"/pdf"};
    }
    
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Send.Document(context, Environment.GetEnvironmentVariable("pathToMaterials") + "conspect.pdf", null, cancellationToken);
        return Globals.GetCommand("getvideo");
    }
}

public class CreativesQuery : Query, IListener
{
    public CreativesQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/creativesForChannel" };
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return Globals.GetCommand("Creatives");
    }
}
public class StoriesPhotoQuery : Query, IListener
{
    public StoriesPhotoQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/storiesPhoto" };
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return Globals.GetCommand("StoriesPhoto");
    }
}
public class StoriesVideoQuery : Query, IListener
{
    public StoriesVideoQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/storiesVideo" };
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return Globals.GetCommand("StoriesVideo");
    }
}
public class VideosQuery : Query, IListener
{
    public VideosQuery(Bot bot) : base(bot)
    {
        Names = new[] { "/videos" };
    }
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return Globals.GetCommand("PromoVideos");
    }
}