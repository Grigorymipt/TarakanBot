using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace WebHookReceiver.Controllers;

[ApiController]
[Route("[controller]")]
public class GetUpdatesController : ControllerBase
{
    private readonly ILogger<GetUpdatesController> _logger;
    public GetUpdatesController(ILogger<GetUpdatesController> logger) => _logger = logger;

    [HttpGet]
    public string GetSendUpdates()
    {
        return "HelloWorld";
    }
    // [HttpPost]
    // public async Task PostSendUpdates(Update update)
    // {
    // }
}