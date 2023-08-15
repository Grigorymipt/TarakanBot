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
    public override Channel GetDocument(long Id)
    {
        var filter = Builders<Channel>.Filter.Eq(u => u.TelegramId, Id);
        return collection.Find(filter).FirstOrDefault();
    }
    public override Channel GetDocument(Guid Id)
    {
        var filter = Builders<Channel>.Filter.Eq(u => u.Id, Id);
        return collection.Find(filter).FirstOrDefault();
    }
    public override async Task<Channel> GetDocumentAsync(long Id)
    { 
        var filter = Builders<Channel>.Filter.Eq(u => u.TelegramId, Id);
        return await collection.Find(filter).FirstOrDefaultAsync<Channel>();
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

    public async Task<List<Channel>> GetOldestDocuments(int count = 20)
    {
        var filter = Builders<Channel>.Filter.Empty;
        var builder = Builders<Channel>.Sort;
        var sort = builder.Ascending(f => f.dateTime);
        var documentList = await collection.Find(filter).Sort(sort).ToListAsync();
        return documentList.GetRange(0, Math.Min(count, documentList.Count));
    }
}