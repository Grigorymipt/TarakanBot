using System.Runtime.CompilerServices;
using MongoDatabase.ModelTG;
using MongoDB.Driver;

namespace MongoDatabase;
public class UserRepository : DocumentRepository<User>
{
    private IMongoCollection<User> collection;

    public UserRepository(string connectionString = null, string databaseName = "admin") : base("User")
    {
        var CollectionRepository = new CollectionRepository(connectionString, databaseName);
        collection = CollectionRepository.GetCollection<User>("User");
    }

    public void CreateDocument(User document)
    {
        collection.InsertOne(document);
    }

    
    public override User GetDocument(long Id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.TelegramId, Id);
        var user = new User();
        user = collection.Find(filter).FirstOrDefault();
        return user;
    }
    public override async Task<User> GetDocumentAsync(long Id)
    { 
        return await Task<User>.Run(() => GetDocument(Id));
    }
    public override User GetDocument(string UserName)
    {
        var filter = Builders<User>.Filter.Eq(u => u.UserName, UserName);
        return collection.Find(filter).FirstOrDefault();
    }
    public override async Task<User> GetDocumentAsync(string UserName)
    {
        return await Task<User>.Run(() => GetDocument(UserName));
    }
    public List<User> GetDocuments(string parent, DateTime dateTime)
    {
        var filter = Builders<User>.Filter.Eq(u => u.RefId, parent);
        var children = collection.FindAsync(filter).Result.ToListAsync().Result.Where(c => c.dateTime > dateTime).ToList();
        return children;
    } 
    public User GetDocumentByTrn(string TrnId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.CurrentPaymentId, TrnId);
        return collection.Find(filter).FirstOrDefault();
    }
}