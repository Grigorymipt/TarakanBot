using System;
using MongoDB.Driver;
using Xunit;
using MongoDatabase;
using MongoDatabase.ModelTG;

namespace MongoDatabaseTests;

public class DatabaseTests
{
    [Fact]
    public void SimpleConnectionTest()
    {
        var cString = Environment.GetEnvironmentVariable("connectionString");
        
        var database = new UserRepository(cString);
        var status = database.GetStatus();
        bool statement = (status == "Connected");
        Assert.True(statement, "status: " + status);
    }
    [Fact]
    public void NullableConnectionString()
    {
        Assert.False(Environment.GetEnvironmentVariable("connectionString")==null,
            "Connection string not found in environmental variables");
    }

    // [Fact]
    // public void CreateAndGetUserTest()
    // {
    //     var cString = Environment.GetEnvironmentVariable("connectionString");
    //     var database = new UserRepository(cString);
    //     User user = new User();
    //     user.Id = new Guid();
    //     database.CreateDocument(user);
    //     var recieved = database.GetDocument(user.Id, "User");
    //     Assert.True(recieved.Id == user.Id, "Errors");
    // }
    [Fact]
    public void CheckCollectionCreator()
    {
        var cString = Environment.GetEnvironmentVariable("connectionString");
        var database = new CollectionRepository(cString);
        var name = "CollectionNameThatWillNeverBeEqualCauseVeryStrangefefeavkicv";
        var index0 = database.GetClient().GetDatabase("admin").ListCollections().ToList().Count;
        database.CreateCollection(name);
        var index1 = database.GetClient().GetDatabase("admin").ListCollections().ToList().Count - 1;
        database.DeleteCollection(name);
        var index2 = database.GetClient().GetDatabase("admin").ListCollections().ToList().Count;
        Assert.True(index0 == index1 && index0 == index2, index0.ToString() + index1.ToString()+ index2.ToString());
    }
}