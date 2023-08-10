using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging.Configuration;
using MyTelegramBot;
using Serilog;

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
        // Task.Run(() => {
        try 
        {
            await handleUpdateService.HandleUpdateAsync(botClient: default, update, cancellationToken);
            _logger.Log(LogLevel.Information, eventId: new EventId(137), message: "Update have handled succesfull");
        }
        catch(Exception ex)
        {
            var ErrorMessage = handleUpdateService.HandleErrorAsync(botClient: default, exception: ex, cancellationToken: cancellationToken).Result;
            _logger.Log(LogLevel.Error, 1312, ErrorMessage);
        }//});
        return Ok();
    }

    [HttpGet]
    [Route("{id?}")]
    public string Get(int? id)
    {
        Environment.SetEnvironmentVariable("VerificationCode", id.ToString());
        return "Use This URL to send a POST query (WebHook)";
    }
}