using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.AspNetCore.Mvc;
using MyTelegramBot;
using Telegram.Bot.Types;

namespace WebHookReceiver.Controllers;

[ApiController]
[Route("[controller]")]
public class GetUpdatesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] Bot handleUpdateService,
        CancellationToken cancellationToken)
    {
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