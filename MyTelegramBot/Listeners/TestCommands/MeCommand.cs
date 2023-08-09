using System.Threading;
using System.Threading.Tasks;
using MyTelegramBot.Types;
using Telegram.Bot.Types;
using MongoDatabase;
using MyTelegramBot.Convertors;
using User = MongoDatabase.ModelTG.User;

namespace MyTelegramBot.Listeners {
    public class MeCommand : Command, IListener {
        public MeCommand(Bot bot): base(bot) {
            Names = new string[]{"/me", "!me"};
        }
        protected override string Run(Context context, CancellationToken cancellationToken)
        {
            var message = context.Update.Message;
            string MessageToSend = base.MessageToSend[0];
            User user;
            if (ArgumentParser.Validate(message.Text, 1))
            {
                long userId;
                string args = ArgumentParser.Parse(message.Text).ArgumentsText;
                long.TryParse(args, out userId);
                var collection = new UserRepository();
                user = collection.GetDocumentAsync(IdConvertor.ToGuid(message.From.Id)).Result; 
                if(user == null)
                {
                    return "User not found";
                }
            }
            else 
            {
                user = Database.GetUserAsync(context.Update.Message.From.Id).Result;
            }

            MessageToSend = "<b>MyTestBot profile</b> \n\n" +
                            $"TG-ID: {user.Id} \n" +
                            $"Channels: \n";
            foreach (var channel in user.Channels)
            {
                MessageToSend += channel + "\n";
            }
            MessageToSend += $"Messages: {user.Messages} \n" ;
            MessageToSend += $"Categories: \n";
            if (user.Categories != null)
            {
                foreach (var category in user.Categories)
                {
                    MessageToSend += Database.GetCategoryAsync(category).Result.Title + "\n";
                }
            }
            return MessageToSend;
        }
        
    }
}