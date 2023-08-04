using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging.Configuration;
using MyTelegramBot;
namespace WebHookReceiver.Controllers;

[ApiController]
[Route("[controller]")]
public class GetUpdatesController : ControllerBase
{
    private readonly ILogger<GetUpdatesController> _logger;
    public GetUpdatesController(ILogger<GetUpdatesController> logger) => _logger = logger;
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] MyTelegramBot.Bot handleUpdateService,
        CancellationToken cancellationToken)
    {
        var updateType = update.GetType();
        Console.WriteLine("TelegramAPIWebhookReceived");
        _logger.Log(LogLevel.Information, eventId: new EventId(), message: $"UpdateType: {updateType}");
        await handleUpdateService.HandleUpdateAsync(botClient: default, update, cancellationToken);
    
        return Ok();
    }

    [HttpGet]
    public string GetSendUpdates()
    {
        return "HelloWorld";
    }
    // [HttpPost]
    // public async Task PostSendUpdates(Update update)
    // {
    //     
    // }
}