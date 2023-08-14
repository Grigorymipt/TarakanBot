using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging.Configuration;
using MyTelegramBot;
using Serilog;


using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Types;
using TL;
using Crypto;

namespace WebHookReceiver.Controllers;

[ApiController]
[Route("[controller]")]
public class GetWalletController : ControllerBase
{
    private readonly ILogger<GetUpdatesController> _logger;
    public GetWalletController(ILogger<GetUpdatesController> logger) => _logger = logger;

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] CreateOrder.ResponseWebHook responseWebHook,
        [FromServices] Bot bot,
        CancellationToken cancellationToken)
    {
        Task.Run(() =>
        {
            try { bot.SuccessPayment(responseWebHook, cancellationToken); }
            catch (Exception ex)
            {
                var ErrorMessage = bot.HandleErrorAsync(botClient: default, exception: ex, cancellationToken: cancellationToken).Result;
                _logger.Log(LogLevel.Error, 1312, ErrorMessage);
            }
        });
        return Ok();
    }
}