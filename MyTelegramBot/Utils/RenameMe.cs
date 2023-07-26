// using System.Reflection;
//
// namespace MyTelegramBot.Utils;
//
// /// <summary>
// ///     Получить словарь свойств объектов с их именами и значениями, если они размечены атрибутом <typeparamref name="T" />
// ///     s
// /// </summary>
// /// <param name="value"></param>
// /// <returns></returns>
// public static IEnumerable<PropertyInfo>GetAttributedPropertiesInfo<T>(this object value)
//     where T : Attribute
// {
//     var result = new Dictionary<string, object?>();
//
//     var type = value.GetType();
//
//     var props = type.GetProperties().Where(
//         prop => Attribute.IsDefined(prop, typeof(T)));
//
//     return props;
// }