using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging.Configuration;
using MyTelegramBot;
namespace WebHookReceiver.Controllers;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class GetUpdatesController : ControllerBase
{
    private readonly ILogger<GetUpdatesController> _logger;
    public GetUpdatesController(ILogger<GetUpdatesController> logger) => _logger = logger;

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] Bot handleUpdateService,
        CancellationToken cancellationToken)
    {
        var text = update.Type switch
        {
            UpdateType.Message => update.Message?.Text?.ToString(),
            UpdateType.CallbackQuery => update.CallbackQuery?.Message?.Text?.ToString(),
            _ => "not recognized"
        };
        Console.WriteLine($"TelegramAPIWebhookReceived: {text}");
        _logger.Log(LogLevel.Information, eventId: new EventId(), message: update.Type.ToString());
        try 
        {
            await handleUpdateService.HandleUpdateAsync(botClient: default, update, cancellationToken);
        }
        catch(Exception ex)
        {
            await handleUpdateService.HandleErrorAsync(botClient: default, exception: ex, cancellationToken: cancellationToken);
            // _logger.Log(LogLevel.Error, 1312, "Handling error", ex);
        }
        return Ok();  
    }

    [HttpGet]
    public string Get()
    {
        return "Use This URL to send a POST query (WebHook)";
    }
}