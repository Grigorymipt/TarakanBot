using MongoDatabase.ModelTG;
using MongoDB.Driver;

namespace MongoDatabase;


public class ChannelRepository : DocumentRepository<Channel>
{
    private IMongoCollection<Channel> collection;

    public ChannelRepository(string connectionString = null, string databaseName = "admin") : base("Channel")
    {
        var CollectionRepository = new CollectionRepository(connectionString, databaseName);
        collection = CollectionRepository.GetCollection<Channel>("Channel");
    }
    public void CreateDocument(Channel document)
    {
        collection.InsertOne(document);
    }
    public override Channel GetDocument(Guid Id)
    {
        var filter = Builders<Channel>.Filter.Eq(u => u.Id, Id);
        return collection.Find(filter).FirstOrDefault();
    }
    public override async Task<Channel> GetDocumentAsync(Guid Id)
    { 
        return await Task<Channel>.Run(() => GetDocument(Id));
    }
    public override Channel GetDocument(string Title)
    {
        var filter = Builders<Channel>.Filter.Eq(u => u.Title, Title);
        return collection.Find(filter).FirstOrDefault();
    }
    public override async Task<Channel> GetDocumentAsync(string Title)
    {
        return await Task<Channel>.Run(() => GetDocument(Title));
    }

    public List<Channel> GetAllDocuments()
    {
        var filter = Builders<Channel>.Filter.Empty;
        return collection.Find(filter).ToList();
    }
    public async Task<List<Channel>> GetAllDocumentsAsync()
    {
        var filter = Builders<Channel>.Filter.Empty;
        var categories = await collection.FindAsync(filter);
        return categories.ToList();
    }
}
