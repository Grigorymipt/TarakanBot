using System.Reflection;
using MyTelegramBot.Utils;

namespace MyTelegramBot;

public static class EnvironmentBinder
{
    public static T Bind<T>()
    {
        var value=Activator.CreateInstance<T>();
        return Bind(value);
    }

    public static IEnumerable<PropertyInfo> GetAttributedPropertiesInfo<T>(this object value)
           where T : Attribute
    {
        var result = new Dictionary<string, object?>();

        var type = value.GetType();

        var props = type.GetProperties().Where(
            prop => Attribute.IsDefined(prop, typeof(T)));

        return props;
    }

    public static T Bind<T>(T valueToBind)
    {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
        var keyValuePairs = valueToBind.GetAttributedPropertiesInfo<EnvironmentVariableKeyAttribute>();
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

        foreach (PropertyInfo propertyToBind in keyValuePairs)
        {
            string variableName = propertyToBind
                .GetCustomAttribute<EnvironmentVariableKeyAttribute>().Name;

            var variable = System.Environment.GetEnvironmentVariable(variableName);
            if (variable == null)
            {
                throw new InvalidOperationException(variable + " is declared but not found");
            }
            propertyToBind.SetValue(valueToBind,variable );
        }
        return valueToBind;
    }
}