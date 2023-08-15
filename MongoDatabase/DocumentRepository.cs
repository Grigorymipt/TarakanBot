using MongoDB.Driver;
using MongoDatabase.ModelTG;
using Amazon.Runtime.Internal.Transform;

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
    public void DeleteDocument(Document document)
    {
        var filter = Builders<Document>.Filter.Eq(u => u.TelegramId, document.TelegramId);
        this.documentCollection.DeleteOne(filter);
    }
    public void DeleteDocument(long documentId)
    {
        var filter = Builders<Document>.Filter.Eq(u => u.TelegramId, documentId);
        this.documentCollection.DeleteOne(filter);
    }
    public async Task DeleteDocumentAsync(Document document)
    {
        await Task.Run(() => DeleteDocument(document));
    }
    
    // FIXME:
    // public void DeleteDocument(long documentId)
    // {
    //     var filter = Builders<Document>.Filter.Eq(u => u.Id, documentId);
    //     this.documentCollection.DeleteOne(filter);
    // }
    // public async Task DeleteDocumentAsync(long documentId)
    // {
    //     await Task.Run(() => DeleteDocument(documentId));
    // }

    public void UpdateDocument(Document oldDocument, Document newDocument)
    {
        newDocument.TelegramId = oldDocument.TelegramId;
        DeleteDocument(oldDocument);
        CreateDocument(newDocument);
    }
    public abstract Document GetDocument(Guid Id);
    public  abstract Document GetDocument(long Id);
    public abstract Document GetDocument(string UserName);
    public abstract Task<T> GetDocumentAsync(long Id);
    public abstract Task<T> GetDocumentAsync(string UserName);

    // public async void Update<T>(Document document)
    // {
    //     var filter = Builders<Document>.Filter.Eq(u => u.Id, document.Id);
    //     var d = typeof(T).Attributes;
    //     foreach (var a in d)
    //     {
    //         
    //     }
    //     var update = Builders<Document>.Update.Set(u => ) 
    //     var updatedDocument = document;
    //     documentCollection.InsertOne(updatedDocument);
    // }
}