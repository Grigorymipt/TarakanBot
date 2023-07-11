using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading;
using MongoDatabase;
using MongoDatabase.ModelTG;
using MyTelegramBot.Convertors;
using Document = MongoDatabase.ModelTG.Document;
using User = MongoDatabase.ModelTG.User;

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
        }
        /// <summary>Checks if the <c>Update</c> matches the listener condition.</summary>
        public abstract bool Validate(Context context, CancellationToken cancellationToken);
        /// <summary>Handles the <c>Update</c> if it is successfully validated.</summary>
        public abstract Task Handler(Context context, CancellationToken cancellationToken);
        /// <returns>The session of the sender of a given <c>Message</c> object.</returns>
        public async Task<User> GetUser(Message message) // TODO: add async here
        {
            var collection = new UserRepository();
            var user = await collection.GetDocumentAsync(IdConvertor.ToGuid(message.From.Id)); 
            if (user == null)
            {
                var document = new User()
                {
                    Id = IdConvertor.ToGuid(message.From.Id)
                };
                await collection.CreateDocumentAsync(document);
                user = await collection.GetDocumentAsync(document.Id);

            }
            return user;
        }
    }
}