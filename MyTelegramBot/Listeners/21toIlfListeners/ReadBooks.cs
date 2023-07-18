using MongoDB.Driver;
using MyTelegramBot;
using MyTelegramBot.Types;

public class ReadBooks : Command // TODO: Query
{
    public ReadBooks(Bot bot) : base(bot)
    {
        Names = new[] { "/readBooks" };
        MessageToSend = "😉 Отлично, тогда лови PDF презентацию! Только, читай внимательно, необходимо ответить на пару" +
                        " вопросов. На всякий случай так же скину короткий ролик обо мне, возможно, он поможет тебе " +
                        "ответить на все вопросы.";
        Buttons = new Dictionary<string, string>() { { "💡 Пройти тест.", "/startTest" } };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Send.Document(context, "/home/pommes/RiderProjects/TelegramBot/Materials/conspect.pdf", cancellationToken);
        return base.Run(context, cancellationToken);
    }
}