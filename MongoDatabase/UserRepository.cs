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
    public async Task CreateDocumentAsync(User document)
    {
        await Task.Run(() => CreateDocument(document));
    } 

    public override User GetDocument(Guid Id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, Id);
        return collection.Find(filter).FirstOrDefault();
    }
    public override async Task<User> GetDocumentAsync(Guid Id)
    {
        return await Task<User>.Run(() => GetDocument(Id));
    }
}