namespace MyTelegramBot.Types;
public record HandleParameters
{
    public string? MessageToSend { get; set; } 
    public Dictionary<string, string> buttons { get; } = new();
    public Dictionary<string, string> links { get; } = new();
}

