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
        public static ILogger ElasticLogger(string indexFormat, string httpUser, string httpPassword,
            string url)
        {
            if (indexFormat == null) throw new ArgumentNullException(nameof(indexFormat));
            if (httpUser == null) throw new ArgumentNullException(nameof(httpUser));
            if (httpPassword == null) throw new ArgumentNullException(nameof(httpPassword));
            if (url == null) throw new ArgumentNullException(nameof(url));
            
            var cfg = LoggingConfigurator.ElasticConfiguration(indexFormat,
                httpUser, httpPassword, url);
            
            var logger = cfg.CreateLogger();

            return logger;
        }

        public static string ToddMMyyyy(this DateTime dt)
        {
            return dt.ToString("ddMMyyyy");
        }

        public static LoggerConfiguration ElasticConfiguration(string indexFormat, string apiUser, string apiKey,
            string url)
        {
            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

            var elasticsearchSinkOptions = new ElasticsearchSinkOptions(new Uri(url))
            {
                ModifyConnectionSettings =
                    x =>x.BasicAuthentication(apiUser, apiKey).EnableApiVersioningHeader(),
                
                AutoRegisterTemplate = true,
                
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7, // < --this was the magic bullet
                IndexFormat = $"{indexFormat}-{DateTime.UtcNow.ToddMMyyyy()}",
                BatchAction = ElasticOpType.Index,
                OverwriteTemplate = true,
                TypeName = null,
                FailureCallback = FailureCallback,

                EmitEventFailure =// EmitEventFailureHandling.WriteToSelfLog |
                    // EmitEventFailureHandling.WriteToFailureSink |
                                  
                    EmitEventFailureHandling.ThrowException,//|
                //     EmitEventFailureHandling.RaiseCallback,
#pragma warning disable CS0618
                FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null)
#pragma warning restore CS0618
            };

            return new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithCorrelationId()
                    .Enrich.WithExceptionData()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithThreadName()
                    .WriteTo.Elasticsearch(elasticsearchSinkOptions)
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
