using Bridge;
using MyTelegramBot;
using Serilog;
using Xunit;

namespace Tests;


public class LoggerTests
{
    [Fact]
    public void LoggerInformationToElasticTest()
    {
        var elkConfiguration = new ELKConfiguration()
        {
            Password = "zevqi1-dawqUk-fixryt",
            Username = "userhub-logger",
            Host = "http://elk.astepanov.space:9200",
        };
        
        var logger =
            LoggingConfigurator.ElasticLogger("userhub",
                elkConfiguration.Username, //todo: завести новое api 
                elkConfiguration.Password,
                elkConfiguration.Host);
        Log.Logger = logger;
        Log.Information("helloELKFromGtest");
    }
}