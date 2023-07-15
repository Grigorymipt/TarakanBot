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
using Telegram.Bot.Types.ReplyMarkups;


namespace MyTelegramBot.Types {
    /// <summary>Abstract Class <c>Listener</c> describes a bot event handler with 
    /// utilities and validate conditions. </summary>
    public abstract class Listener
    {
        /// <value>
        /// Property <c>Bot</c> represents a <c>Bot</c> instance with which <c>Listener</c> is related.
        /// </value>
        public Bot Bot { get; set; }


        public HandleType HandleType { get; set; } = HandleType.Standard;
        private Dictionary<string, string> buttons;
        public Dictionary<string, string> Buttons
        {
            get => buttons;
            set
            {
                buttons = value;
                HandleType = HandleType.ButtonList;
            }
        }

        private string filePath;
        public bool fileToSend { get; set; } = false;
        public string MessageToSend { get; set; } = "This command is under development and not currently available.";
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                fileToSend = true;
            }
        }

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

        public abstract Task Handler(Context context, Dictionary<string, string> buttonsList,
            CancellationToken cancellationToken);
        
        
        
        /// <returns>The session of the sender of a given <c>Message</c> object.</returns>
        protected async Task<User> GetUser(long Id) // TODO: add async here
        {
            var collection = new UserRepository();
            User user = await collection.GetDocumentAsync(IdConvertor.ToGuid(Id));
            return user;
        }
        protected User GetUserSync(long Id) // TODO: add async here
        {
            var collection = new UserRepository();
            User user = collection.GetDocument(IdConvertor.ToGuid(Id));
            return user;
        }

        public User CreateUser(Message message)
        {
            var collection = new UserRepository();
            var parent = collection.GetDocument(ArgumentParser.Parse(message.Text).ArgumentsText);
            
            var user = new User()
            {
                Id = IdConvertor.ToGuid(message.From.Id),
                UserName = message.From.Username
            };
            if (parent != null)
            {
                Console.WriteLine(parent.UserName);
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
            var user = GetUserSync(message.From.Id);
            if (parent != null)
            {
                string parentUserName = parent.UserName;
                user.RefId = parentUserName;
            }
            else user.RefId = null;
            user.Update();
            return user;
        }
        /// <summary>Processes a command synchronously.</summary>
        /// <returns>Command result string.</returns>
        public virtual string Run(Context context, CancellationToken cancellationToken) {
            return MessageToSend;
        }
        /// <summary>Processes a command asynchronously.</summary>
        /// <returns>Command result string.</returns>
        public virtual async Task<string> RunAsync(Context context, CancellationToken cancellationToken) {
            return Run(context, cancellationToken);
        }
    }
}