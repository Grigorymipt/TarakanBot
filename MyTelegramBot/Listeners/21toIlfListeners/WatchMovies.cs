using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;
using InlineQuery = Telegram.Bot.Types.InlineQuery;

namespace MyTelegramBot.Listeners._21toIlfListeners;

public class WatchMovies : Query, IListener // TODO: Query, IListener
{
    public WatchMovies(Bot bot) : base(bot)
    {
        Names = new[] { "/watchmovies", "/watchMovies" };
        MessageToSend = new string[] {Globals.responses.GetValueOrDefault("get video")
                        };
    }

    // public override async Task Handler(Context context, CancellationToken cancellationToken)
    // {
    //     MessageToSend += "\n https://www.youtube.com/watch?v=A0_Abt4dzAA";
    //     await base.Handler(context, cancellationToken);
    //     // Int64 chatId = context.Update.Message.Chat.Id;
    //     // var url = new Uri();
    //     // InputFile video = new InputFileStream();
    //     // await context.BotClient.SendVideoAsync(
    //     //     chatId: chatId,
    //     //     video: video
    //     // );
    // }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>( ){ { "Посмотрел, отправить кодовое слово", "/sendKeyWord" } };
        return MessageToSend + " https://www.youtube.com/watch?v=A0_Abt4dzAA";
    }
}

public class SendKeyWord : Types.InlineQuery, IListener
{
    public SendKeyWord(Bot bot) : base(bot)
    {
        Names = new[] { "/sendKeyWord" };
        MessageToSend = new string[] {"Кодовое слово из видео:"};
        MessageLabel = "KeyWord";
    }
}

public class GetKeyWord : Types.InlineReply, IListener
{
    public GetKeyWord(Bot bot) : base(bot)
    {
        MessageToSend = new string[]{
            Globals.responses.GetValueOrDefault("getpdf"),
            Globals.responses.GetValueOrDefault("watchagain"),
            Globals.responses.GetValueOrDefault("mbbooks")
        };
        MessageLabel = "KeyWord";
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>();
        if (context.Update.Message.Text.Equals("Миллион") || context.Update.Message.Text.Equals("миллион"))
        {
            Buttons.Clear();
            Buttons.Add(Globals.responses.GetValueOrDefault("passtest"), "/startTest");
            return MessageToSend[0];
        }
        else
        {
            var user = Database.GetUser(context.Update.Message.From.Id);
            user.Attempts += 1;
            user.Update();
            if (user.Attempts < 2)
            {
                Buttons.Clear();
                Buttons.Add(Globals.responses.GetValueOrDefault("wrongagain"), "/sendKeyWord");
                return MessageToSend[1];
            }
            else
            {
                Buttons.Clear();
                Buttons.Add(Globals.responses.GetValueOrDefault("passtest"), "/startTest");
                ChatId chatId = context.Update.Message.Chat.Id;
                Send.Document(context, Environment.GetEnvironmentVariable("pathToMaterials")+"conspect.pdf", cancellationToken);
                return MessageToSend[2];
            }
        }
    }
}