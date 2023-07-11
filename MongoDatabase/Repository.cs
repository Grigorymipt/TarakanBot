using MongoDB.Driver;

namespace MongoDatabase;

public class Repository
{
    private readonly string _connectionString;
    protected MongoClient client { get; set; }
    public IMongoClient GetClient()
    {
        return client;
    }
    protected StatusDatabase status;
    protected string databaseName;
    public string GetStatus()
    {
        return status.ToString();
    }

    public Repository(string connectionString = null, string databaseName = "admin")
    {
        _connectionString = connectionString;
        this.databaseName = databaseName;
        try
        {
            if (_connectionString == null)
            {
                var conString = Environment.GetEnvironmentVariable("connectionString");
                if (conString == null) throw new ArgumentNullException("Connection string is null - not found");
                _connectionString = conString;
            }
        }
        catch
        {
            _connectionString = null;
        }
        finally
        {
            ConnectDatabase();
        }
    }
    private IMongoClient ConnectDatabase()
    {
        try
        {
            client = new MongoClient(_connectionString);
            status = StatusDatabase.Connected;
        }
        catch(Exception exception)
        {
            status = StatusDatabase.ConnectionFailed;
            Console.WriteLine(exception);
        }
        return client;
    }
}