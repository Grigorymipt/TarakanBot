using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Telegram.Bot;
namespace MyTelegramBot;

public static class WebHook
{
    public static async Task UpdateReceive(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddScoped<Bot>();
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Listen(IPAddress.Any, 8443, options =>
            {
                options.UseHttps();
                options.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
            });
            serverOptions.Listen(IPAddress.Any, 8080, options =>
            {
                options.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
            });
        });
        Console.WriteLine("---------------------!!!!!!!!!!!!!!!!_----------------");
        // builder.Services.AddHttpsRedirection(options =>
        // {
        //     options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
        //     options.HttpsPort = 443;
        // });
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
}

