// using System.Reflection;
//
// namespace MyTelegramBot.Utils;
//
// public static T Bind<T>(T valueToBind)
// {
//     var keyValuePairs = valueToBind.GetAttributedPropertiesInfo<EnvironmentVariableKeyAttribute>();
//         
//     foreach (var propertyToBind in keyValuePairs)
//     {
//         PropertyInfo? propertyInfo = propertyToBind;
//         string variableName = propertyInfo
//             .GetCustomAttribute<EnvironmentVariableKeyAttribute>().Name;
//
//         var variable = System.Environment.GetEnvironmentVariable(variableName);
//         if (variable == null)
//         {
//             throw new InvalidOperationException(variable + " is declared but not found");
//         }
//         propertyInfo.SetValue(valueToBind,variable );
//     }
//     return valueToBind;
// }