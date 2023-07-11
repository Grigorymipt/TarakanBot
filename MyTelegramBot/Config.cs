using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot {
    public static class Config {
        public static ParseMode ParseMode { get; } = ParseMode.Html;
        public static string BotToken { get => Configuration["BotToken"]; }
        // public static string DbString { get => Configuration.GetConnectionString("DefaultConnection"); }
        // public static string DbProvider { get => Configuration.GetSection("Database")?["Provider"]?.ToLower() ?? "sqlite"; }
        
        // Common
        public static IConfiguration Configuration { get; set; }
        static Config()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
    }
}