using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MyTelegramBot.Convertors;
using MyTelegramBot.Utils;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace MyTelegramBot.Types;

public class Database
{
    readonly static MongoClient mongoClient = new(Environment.GetEnvironmentVariable("ConnectionString"));
    readonly static IMongoDatabase mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));

    public static async Task<User> GetUser(Message message) => await GetDocument<User, string>(u => u.TelegramId, message.From.Id.ToString());
    public static async Task<User> GetUser(long Id) => await GetDocument<User, string>(u => u.TelegramId, Id.ToString());
    
    public static async Task CreateDocument<TDocument>(TDocument document) 
        => await mongoDatabase.GetCollection<TDocument>(typeof(TDocument).ToString()).InsertOneAsync(document);
    
    public static async Task<TDocument> GetDocument<TDocument, TField>(Expression<Func<TDocument, TField>> findExpression, TField Id)
        => await mongoDatabase.GetCollection<TDocument>(typeof(TDocument).ToString()).FindAsync(GetFilter(findExpression, Id)).Result.FirstAsync();

    public static async Task<List<TDocument>> GetDocumentMany<TDocument, TField>(Expression<Func<TDocument, TField>> findExpression, TField Id)
        => mongoDatabase.GetCollection<TDocument>(typeof(TDocument).ToString()).FindAsync(GetFilter(findExpression, Id)).Result.ToList();
    public static async Task UpdateDocument<TDocument, TFindField, TUpdateField>(
        Expression<Func<TDocument, TFindField>> findExpression, TFindField Id,
        Expression<Func<TDocument, TUpdateField>> updateExpression, List<TUpdateField> updates) 
    {    
        foreach (var update in updates)
            await mongoDatabase.GetCollection<TDocument>(typeof(TDocument).ToString())
                .UpdateOneAsync(GetFilter(findExpression, Id), GetUpdates(updateExpression, update));
    }

    public static async Task DeleteDocument<TDocument, TField>(Expression<Func<TDocument, TField>> findExpression, TField Id)
        => await mongoDatabase.GetCollection<TDocument>(typeof(TDocument).ToString()).DeleteOneAsync(GetFilter(findExpression, Id));

    private static FilterDefinition<TDocument> GetFilter<TDocument, TField>(
        Expression<Func<TDocument, TField>> expression, TField Id) 
        => Builders<TDocument>.Filter.Eq(expression, Id);
    
    private static UpdateDefinition<TDocument> GetUpdates<TDocument, TField>(
        Expression<Func<TDocument, TField>> expression, TField update)
        => Builders<TDocument>.Update.SetOnInsert(expression, update);
    
}