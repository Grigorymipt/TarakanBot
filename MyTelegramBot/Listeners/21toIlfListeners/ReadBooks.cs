using MongoDB.Driver;
using MyTelegramBot;
using MyTelegramBot.Types;

public class ReadBooks : Query, IListener // TODO: Query, IListener
{
    public ReadBooks(Bot bot) : base(bot)
    {
        Names = new[] { "/readBooks" };
        MessageToSend = new[] {Globals.responses.GetValueOrDefault("getbook")};
    }

    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> buttons)
    {
        buttons = new Dictionary<string, string>() { { "💡 Пройти тест.", "/startTest" } };
        Send.Document(context, Environment.GetEnvironmentVariable("pathToMaterials")+"conspect.pdf", cancellationToken);
        return MessageToSend[0];
    }
}