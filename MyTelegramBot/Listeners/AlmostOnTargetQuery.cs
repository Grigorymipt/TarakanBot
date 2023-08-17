using MongoDatabase.ModelTG;
using MyTelegramBot.Types;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TL.Methods;

namespace MyTelegramBot.Listeners;

public class AlmostOnTargetQuery : InlineReply, IListener 
{
    public AlmostOnTargetQuery(Bot bot) : base(bot)
    {
        MessageToSend = new[] {Globals.GetCommand("laststep")};
        MessageLabel = "GetAddressInline";
    }
    protected override string Run(Context context, CancellationToken cancellationToken, out Dictionary<string, string> Buttons)
    {
        Buttons = new Dictionary<string, string>()
        {
            {Globals.GetCommand("sugaccepted"), "/suggestionAccepted"},
            {Globals.GetCommand("payforlist"), "/buyListingNow"}
        };
        // Console.WriteLine(context.Update.Message.From.Id);
        
        Send.Photo(context, Environment.GetEnvironmentVariable("pathToMaterials") + "repost.jpg", cancellationToken);

        var user = Database.GetUser(context.Update.Message.From.Id);
        // Console.WriteLine(context.Update.Message.Text);
        string newChannel = context.Update.Message.Text;
        if (newChannel.First() != '@')
        {
            Buttons.Clear();
            Buttons.Add(Globals.GetCommand("SendChannelNameAgain"),"/saveCategory");
            return Globals.GetCommand("InvalidChannelName");
        }
        var newUser = user;
        newChannel = newChannel.Remove(0, 1);
        try
        {
            if (context.BotClient.IsAdmin("@" + newChannel, context.Update.Message.From.Id).Result) 
            {
                if(newUser.Channels == null || newUser.Channels.Contains(newChannel) == false)
                {
                    newUser.Channels ??= new List<string>();
                    newUser.Channels.Add(newChannel); // FIXME: very strange behavior
                    Channel channel = new Channel()
                    {
                        Owner = user.TelegramId,
                        Title = newChannel,
                    };
                    Database.CreateChannel(channel);
                }
                else
                {
                    Buttons.Clear();
                    return "Вы уже добавляли данный канал";
                }
            }
            else
            {
                Buttons.Clear();
                return "Вы не являетесь создателем данного канала";
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            Buttons.Clear();
            Buttons.Add("Попробовать еще раз", "/saveCategory");
            if (ex.Message == "Channel not Exists")
            {
                return "Такого канала не существует";
            }
            else if(ex is System.AggregateException aex)
            {
                return "Добавьте наш бот админом с правами отправки сообщений в ваш канал, после нажмите 'Попробовать еще раз'";
            }
            return "Unexpected Error";
            throw;
        }
        newUser.LastMessage = null;
        newUser.Update();
        return MessageToSend[0];
    }
}