using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading;
using MongoDatabase;
using MongoDatabase.ModelTG;
using MyTelegramBot.Convertors;
using Document = MongoDatabase.ModelTG.Document;
using User = MongoDatabase.ModelTG.User;
using MyTelegramBot.Types;
using MyTelegramBot.Utils;


namespace MyTelegramBot.Types {
    /// <summary>Abstract Class <c>Listener</c> describes a bot event handler with 
    /// utilities and validate conditions. </summary>
    public abstract class Listener
    {
        /// <value>
        /// Property <c>Bot</c> represents a <c>Bot</c> instance with which <c>Listener</c> is related.
        /// </value>
        public Bot Bot { get; set; }
        /// <summary>
        ///  Creates a <c>Listener</c> for the specified <c>Bot</c>.
        /// </summary>
        public Listener(Bot bot)
        {
            Bot = bot;
            ArgumentParser = new CommandParser();
        }
        public CommandParser ArgumentParser { get; set; }
        /// <summary>Checks if the <c>Update</c> matches the listener condition.</summary>
        public abstract Task<bool> Validate(Context context, CancellationToken cancellationToken);
        /// <summary>Handles the <c>Update</c> if it is successfully validated.</summary>
        public abstract Task Handler(Context context, CancellationToken cancellationToken);
        /// <returns>The session of the sender of a given <c>Message</c> object.</returns>
        protected async Task<User> GetUser(Message message) // TODO: add async here
        {
            var collection = new UserRepository();
            User user = await collection.GetDocumentAsync(message.From.Username);
            return user;
        }
        protected User GetUserSync(Message message) // TODO: add async here
        {
            var collection = new UserRepository();
            User user = collection.GetDocument(message.From.Username);
            return user;
        }

        public User CreateUser(Message message)
        {
            var collection = new UserRepository();
            Console.WriteLine(message.Text);
            var parent = collection.GetDocument(ArgumentParser.Parse(message.Text).ArgumentsText);
            var user = new User();
            if (parent != null)
            {
                string parentUserName = parent.UserName;
                user.RefId = parentUserName;
            }
            else user.RefId = null;
            collection.CreateDocument(user);
            return user;
        }

        public User UpdateUser(Message message)
        {
            var collection = new UserRepository();
            Console.WriteLine(message.Text);
            var parent = collection.GetDocument(ArgumentParser.Parse(message.Text).ArgumentsText);
            var user = GetUserSync(message);
            if (parent != null)
            {
                string parentUserName = parent.UserName;
                user.RefId = parentUserName;
            }
            else user.RefId = null;
            user.Update();
            return user;
        }
    }
}