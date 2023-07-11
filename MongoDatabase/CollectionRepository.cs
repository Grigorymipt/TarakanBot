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
    }

    public void CreateCollection(string collectionName)
    {
        client.GetDatabase(databaseName).CreateCollection(collectionName);
        Console.WriteLine("Collection successfully created!");
    }
    
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return client.GetDatabase(databaseName).GetCollection<T>(collectionName);
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