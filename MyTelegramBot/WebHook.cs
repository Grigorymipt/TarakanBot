using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;

namespace MyTelegramBot;

public static class WebHook
{
    public static async Task UpdateReceive(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Setup Bot configuration
        var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
        builder.Services.Configure<BotConfiguration>(botConfigurationSection);
        var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

        // Add services to the container.
        builder.Services.AddScoped<Bot>();
        builder.Services.AddControllers().AddNewtonsoftJson();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>(httpClient =>
                {
                    TelegramBotClientOptions options = new(Config.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Listen(IPAddress.Any, 8080, options =>
            {
                options.Protocols = HttpProtocols.Http1AndHttp2;
            });
            serverOptions.Listen(IPAddress.Any, 80, options =>
            {
                options.Protocols = HttpProtocols.Http1AndHttp2;
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseHsts();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
    public class BotConfiguration
    {
        public static readonly string Configuration = "BotConfiguration";

        public string BotToken { get; init; } = default!;
        public string HostAddress { get; init; } = default!;
        public string Route { get; init; } = default!;
        public string SecretToken { get; init; } = default!;
    }
}


