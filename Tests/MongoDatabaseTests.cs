using System;
using Crypto;
using MongoDB.Driver;
using Xunit;
using MongoDatabase;
using MongoDatabase.ModelTG;
using MyTelegramBot.Convertors;
using MyTelegramBot.Types;
using Telegram.Bot.Requests.Abstractions;

namespace Tests;

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
    // [Fact]
    // public void NullableConnectionString()
    // {
    //     Assert.False(Environment.GetEnvironmentVariable("connectionString")==null,
    //         "Connection string not found in environmental variables");
    // }

    // [Fact]
    // public void CreateAndGetUserTest()
    // {
    //     var cString = Environment.GetEnvironmentVariable("connectionString");
    //     var database = new UserRepository(cString);
    //     User user = new User();
    //     user.Id = new long();
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
        Assert.True(index0 == index1 && index0 == index2, index0 + index1.ToString() + index2);
    }

    [Fact]
    public void DeleteAllKHEram()
    {
        var cString = Environment.GetEnvironmentVariable("connectionString");
        var database = new UserRepository(cString);
        var collec = new CollectionRepository();
        // FIXME: uncomment!!!!
        collec.DeleteCollection("User");
        User user = new User();
        user.Id = new long();
        user.UserName = "SuperUser";
        database.CreateDocument(user);
        Console.WriteLine(database.GetDocumentAsync(user.Id).Result.UserName);
        
        User confirmUser = new User();
        confirmUser.Id = -11;
        //vercode:
        long verificationCode = 22895;
        confirmUser.LastMessage = verificationCode.ToString();
        database.DeleteDocument(-11);
        // database.CreateDocument(confirmUser);
    }

    // [Fact]
    // public void addDB()
    // {
    //     var cString = Environment.GetEnvironmentVariable("connectionString");
    //     Repository repository = new Repository(cString);
    // }


    // [Fact]
    // public async void listAll()
    // {
    //     var cString = Environment.GetEnvironmentVariable("connectionString");
    //     var database = new UserRepository(cString);
    //     var god = await database.GetDocumentAsync("SuperUser");
    //     Console.WriteLine(god.Id + " " + god.UserName + god.RefId);
    //     var good = await database.GetDocumentAsync("g_koveshnikov");
    //     Console.WriteLine(good.Id + " " + good.UserName + good.RefId);
    // }
    [Fact]
    public void GetChannels()
    {
        var user = new User();
        string newChannel = "smth";
        Console.WriteLine(user.Channels);
        user.Channels.Add(newChannel);
    }

    [Fact]
    public void EnumTest()
    {
        Console.WriteLine(Period.Day.ToString());   
    }
    public enum Period 
    {
        Day,
        Week,
        Month,
        Quarter,
        Year,
        All,
    }

    [Fact]
    public void ChannelInfoTest()
    {
        Assert.True(
            ChannelInfo.Subscribed(
                "TestForTestingAndTestingForTest",
                3928432
                ).Result == false
            );
    }
}
