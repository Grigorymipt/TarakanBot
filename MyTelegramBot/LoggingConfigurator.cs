using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using ILogger = Serilog.ILogger;

namespace Bridge
{
    public static class LoggingConfigurator
    {
        public static ILogger ElasticLogger(string indexFormat)
        {
            if (indexFormat == null) throw new ArgumentNullException(nameof(indexFormat));
            
            var cfg = LoggingConfigurator.ElasticConfiguration();
            
            var logger = cfg.CreateLogger();

            return logger;
        }

        public static string ToddMMyyyy(this DateTime dt)
        {
            return dt.ToString("ddMMyyyy");
        }

        public static LoggerConfiguration ElasticConfiguration()
        {
            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

            return new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithCorrelationId()
                    .Enrich.WithExceptionData()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithThreadName()
                    .WriteTo.Console();
        }

        private static void FailureCallback(LogEvent e)
        {
            Console.WriteLine("Unable to submit event " + e.MessageTemplate);
            Log.Information("Unable to submit event " + e.MessageTemplate);
            // Assert.Fail();
            throw (e.Exception);
        }
    }
}
