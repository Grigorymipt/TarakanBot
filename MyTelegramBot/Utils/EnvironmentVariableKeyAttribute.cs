namespace MyTelegramBot.Utils;

public class EnvironmentVariableKeyAttribute : Attribute
{
    public EnvironmentVariableKeyAttribute(string name)
    {
        Name = name;
    }
    public string Name { get; private set; }
}