using MongoDB.Driver;
using MongoDatabase.ModelTG;


namespace MongoDatabase;

public abstract class DocumentRepository<T> : Repository
{
    private IMongoCollection<Document> documentCollection;

    public DocumentRepository(string collectionName, string connectionString = null, string databaseName = "admin") 
        : base(connectionString, databaseName)
    {
        var CollectionRepository = new CollectionRepository(connectionString, base.databaseName);
        documentCollection = CollectionRepository.GetCollection<Document>(collectionName);
    }
    private  void  CreateDocument(Document document)
    {
        documentCollection.InsertOne(document);
    }
    private async Task CreateDocumentAsync(Document document)
    {
        await Task.Run(() => CreateDocument(document));
    }

    public void DeleteDocument(Document document)
    {
        var filter = Builders<Document>.Filter.Eq(u => u.Id, document.Id);
        this.documentCollection.DeleteOne(filter);
    }
    public async Task DeleteDocumentAsync(Document document)
    {
        await Task.Run(() => DeleteDocument(document));
    }
    
    // FIXME:
    // public void DeleteDocument(Guid documentId)
    // {
    //     var filter = Builders<Document>.Filter.Eq(u => u.Id, documentId);
    //     this.documentCollection.DeleteOne(filter);
    // }
    // public async Task DeleteDocumentAsync(Guid documentId)
    // {
    //     await Task.Run(() => DeleteDocument(documentId));
    // }

    public void UpdateDocument(Document oldDocument, Document newDocument)
    {
        newDocument.Id = oldDocument.Id;
        DeleteDocument(oldDocument);
        CreateDocument(newDocument);
    }
    public async Task UpdateDocumentAsync(Document oldDocument, Document newDocument)
    {
        await Task.Run(() => UpdateDocument(oldDocument, newDocument));
    }

    public abstract Document GetDocument(Guid Id);
    public abstract Task<T> GetDocumentAsync(Guid Id);
}