using MyTelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;
using InlineQuery = Telegram.Bot.Types.InlineQuery;

namespace MyTelegramBot.Listeners._21toIlfListeners;

public class WatchMovies : Command, IListener // TODO: Query, IListener
{
    public WatchMovies(Bot bot) : base(bot)
    {
        Names = new[] { "/watchmovies", "/watchMovies" };
        MessageToSend = "😉 Отлично, тогда лови короткое видео обо мне. Смотри внимательно, в видео спрятано кодовое" +
                        " слово! Нужно будет отправить его мне, чтобы перейти к следующему шагу.";
        Buttons = new Dictionary<string, string>( ){ { "Посмотрел, отправить кодовое слово", "/sendKeyWord" } };
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
    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        MessageToSend += "https://www.youtube.com/watch?v=A0_Abt4dzAA";
        return base.Run(context, cancellationToken);
    }
}

public class SendKeyWord : Types.InlineQuery, IListener
{
    public SendKeyWord(Bot bot) : base(bot)
    {
        Names = new[] { "/sendKeyWord" };
        MessageToSend = "Кодовое слово из видео:";
        MessageLabel = "KeyWord";
    }
}

public class GetKeyWord : Types.InlineReply, IListener
{
    public GetKeyWord(Bot bot) : base(bot)
    {
        MessageLabel = "KeyWord";
        Buttons = new Dictionary<string, string>();
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Message.Text.Equals("Миллион") || context.Update.Message.Text.Equals("миллион"))
        {
            MessageToSend = "🥳 Правильно! Ты почти у цели. Дополнительно лови PDF презентацию! Здесь ты найдешь" +
                            " ответы на все вопросы. Жми на 'пройти тест' 👇";
            Buttons.Clear();
            Buttons.Add("💡 Пройти тест.", "/startTest");
        }
        else
        {
            var user = Database.GetUser(context.Update.Message.From.Id);
            user.Attempts += 1;
            user.Update();
            if (user.Attempts < 2)
            {
                MessageToSend = "😎 Соберись, всего несколько шагов отделяет тебя от первого 1.000.000 подписчиков. " +
                                "Еще раз посмотри внимательно видео! (подсказка, с 00:00 сек до 00:00 сек)";
                Buttons.Clear();
                Buttons.Add("Попробовать еще раз", "/sendKeyWord");
            }
            else
            {
                MessageToSend = "😳 Возможно, книги ты любишь больше! Тогда лови PDF презентацию. Только, читай" +
                                " внимательно, необходимо будет ответить на пару вопросов.";
                Buttons.Clear();
                Buttons.Add("💡 Пройти тест.", "/startTest");
                ChatId chatId = context.Update.Message.Chat.Id;
                Send.Document(context, Environment.GetEnvironmentVariable("pathToMaterials")+"conspect.pdf", cancellationToken);
            }
        }
        return base.Run(context, cancellationToken);
    }
}