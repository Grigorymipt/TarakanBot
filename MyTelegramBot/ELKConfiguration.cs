using MyTelegramBot.Utils;

namespace MyTelegramBot;

public class ELKConfiguration
{
    [EnvironmentVariableKey("ELK_USER")]
    public string Username { get; set; }
    
    [EnvironmentVariableKey("ELK_PASSWORD")]
    public string Password { get; set; }
    
    [EnvironmentVariableKey("ELK_HOST")]
    public string Host { get; set; }
}