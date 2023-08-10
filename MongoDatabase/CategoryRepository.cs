using MongoDatabase.ModelTG;
using MongoDB.Driver;

namespace MongoDatabase;

public class CategoryRepository : DocumentRepository<Category>
{
    private IMongoCollection<Category> collection;

    public CategoryRepository(string connectionString = null, string databaseName = "admin") : base("Category")
    {
        var CollectionRepository = new CollectionRepository(connectionString, databaseName);
        collection = CollectionRepository.GetCollection<Category>("Category");
    }
    public void CreateDocument(Category document)
    {
        collection.InsertOne(document);
    }
    public override Category GetDocument(long Id)
    {
        var filter = Builders<Category>.Filter.Eq(u => u.DocumentId, Id);
        return collection.Find(filter).FirstOrDefault();
    }
    public override async Task<Category> GetDocumentAsync(long Id)
    { 
        return await Task<Category>.Run(() => GetDocument(Id));
    }
    public override Category GetDocument(string Title)
    {
        var filter = Builders<Category>.Filter.Eq(u => u.Title, Title);
        return collection.Find(filter).FirstOrDefault();
    }
    public override async Task<Category> GetDocumentAsync(string Title)
    {
        return await Task<Category>.Run(() => GetDocument(Title));
    }

    public List<Category> GetAllDocuments()
    {
        var filter = Builders<Category>.Filter.Empty;
        return collection.Find(filter).ToList();
    }
    public async Task<List<Category>> GetAllDocumentsAsync()
    {
        var filter = Builders<Category>.Filter.Empty;
        var categories = await collection.FindAsync(filter);
        return categories.ToList();
    }
}