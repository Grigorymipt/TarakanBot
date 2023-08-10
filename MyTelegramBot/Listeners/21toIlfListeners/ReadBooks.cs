using MongoDB.Driver;
using MyTelegramBot;
using MyTelegramBot.Types;

public class ReadBooks : Command, IListener // TODO: Query, IListener
{
    public ReadBooks(Bot bot) : base(bot)
    {
        Names = new[] { "/readBooks" };
        MessageToSend = new[] {"😉 Отлично, тогда лови PDF презентацию! Только, читай внимательно, необходимо ответить на пару" +
                        " вопросов. На всякий случай так же скину короткий ролик обо мне, возможно, он поможет тебе " +
                        "ответить на все вопросы."};
        
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>() { { "💡 Пройти тест.", "/startTest" } };
        Send.Document(context, Environment.GetEnvironmentVariable("pathToMaterials")+"conspect.pdf", cancellationToken);
        return MessageToSend[0];
    }
}