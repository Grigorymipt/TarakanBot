using System.Data.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.IO;
using System.Reflection;
using System;
using MongoDatabase.ModelTG;

namespace MongoDatabase;
public class CollectionRepository : Repository
{
    //private Type DocumentType;
    private readonly string DB_Name;

    /// <summary>
    /// Create DB client for - connection with DB
    /// </summary>``
    /// <returns></returns>
    public CollectionRepository(string connectionString = null, string databaseName = "admin") 
        : base(connectionString, databaseName)
    {
        var collections = client.GetDatabase(base.databaseName).ListCollectionNames().ToList();
        List<string> necessaryCollections = new List<string>(){"User", "Channel", "Category"};
        foreach (var necessaryCollection in necessaryCollections)
        {
            try
            {
                if (collections.Contains(necessaryCollection) == false)
                    throw new NullReferenceException($"Collection {necessaryCollection} does not exist.");
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                CreateCollection(necessaryCollection);
            }
        }
    }

    public void CreateCollection(string collectionName)
    {
        client.GetDatabase(databaseName).CreateCollection(collectionName);
        Console.WriteLine("Collection successfully created!");
    }
    
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        var collection = client.GetDatabase(databaseName).GetCollection<T>(collectionName);
        return collection;
    }
    public void DeleteCollection(string collectionName)
    {
        client.GetDatabase(databaseName).DropCollection(collectionName);
    }
}




class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(0);
    }
}