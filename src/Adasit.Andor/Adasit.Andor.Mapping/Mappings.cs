using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.ValuesObjects;
using System.Collections;
using System.Reflection;

namespace Adasit.Andor.Mapping;

public class Mappings
{
    public static T GetValid<T, R>(R entity)
    where T : class
    where R : class
    {
        var dictionary = new Dictionary<string, object?>();

        foreach (var property in typeof(T).GetProperties(
                     BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var sourceProperty = typeof(R).GetProperty(property.Name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (sourceProperty != null)
            {
                var value = sourceProperty.GetValue(entity);
                dictionary.Add(property.Name, value);
            }
            else
            {
                dictionary.Add(property.Name, null);
            }
        }

        return CreateInstanceAndSetProperties<T>(dictionary)!;
    }


    public static T? CreateInstanceAndSetProperties<T>(Dictionary<string, object?> propertyValues) where T : class
    {
        var type = typeof(T);

        var instance = (T?)Activator.CreateInstance(type, true);

        if (instance == null)
            return null;

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (!property.CanWrite)
                continue;

            if (propertyValues.TryGetValue(property.Name, out var value))
            {
                if (IsIId(property.PropertyType) && value is Guid guidValue)
                {
                    var loadMethod = property.PropertyType.GetMethod("Load", new[] { typeof(Guid) });
                    var idInstance = loadMethod?.Invoke(null, new object[] { guidValue });
                    property.SetValue(instance, idInstance);
                    continue;
                }

                if (IsStringValueObject(property.PropertyType) && value is string strValue)
                {
                    var voInstance = Activator.CreateInstance(
                        property.PropertyType,
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        new object[] { strValue },
                        null);

                    property.SetValue(instance, voInstance);
                    continue;
                }

                if (IsEnumeration(property.PropertyType) && value is not null)
                {
                    var keyType = property.PropertyType.BaseType!.GetGenericArguments()[0];

                    // Converte o valor para o tipo de chave (int, string ou Guid)
                    var convertedKey = Convert.ChangeType(value, keyType);

                    // Pega o método estático GetByKey<T>(TKey key)
                    var getByKeyMethod = typeof(Enumeration<>)
                        .MakeGenericType(keyType)
                        .GetMethod(nameof(Enumeration<object>.GetByKey), BindingFlags.Public | BindingFlags.Static)!
                        .MakeGenericMethod(property.PropertyType);

                    var enumInstance = getByKeyMethod.Invoke(null, new[] { convertedKey });

                    property.SetValue(instance, enumInstance);
                    continue;
                }

                if (IsCollection(property.PropertyType) && value is IEnumerable<object> enumerable)
                {
                    var elementType = property.PropertyType.GetGenericArguments()[0];
                    var listType = typeof(List<>).MakeGenericType(elementType);
                    var listInstance = (IList)Activator.CreateInstance(listType)!;

                    foreach (var item in enumerable)
                    {
                        if (item == null)
                        {
                            listInstance.Add(null);
                            continue;
                        }

                        var nestedInstance = typeof(Mappings)
                            .GetMethod(nameof(GetValid))!
                            .MakeGenericMethod(elementType, item.GetType())
                            .Invoke(null, new object[] { item });

                        listInstance.Add(nestedInstance);
                    }

                    property.SetValue(instance, listInstance);
                    continue;
                }

                if (!property.PropertyType.IsPrimitive &&
                    property.PropertyType != typeof(string) &&
                    !property.PropertyType.IsEnum &&
                    !IsGuid(property.PropertyType) &&
                    !IsDateTime(property.PropertyType) &&
                    value is not null)
                {
                    var nestedProps = value.GetType().GetProperties()
                        .ToDictionary(p => p.Name, p => p.GetValue(value));

                    var nestedInstance = typeof(Mappings)
                    .GetMethod(nameof(GetValid))!
                    .MakeGenericMethod(property.PropertyType, value.GetType())
                    .Invoke(null, new object[] { value });

                    property.SetValue(instance, nestedInstance);
                    continue;
                }

                property.SetValue(instance, value);
            }
        }

        return instance;
    }

    private static bool IsIId(Type type)
    {
        return type.GetInterfaces()
                   .Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IId<>)));
    }

    private static bool IsStringValueObject(Type type)
    {
        return typeof(StringValueObject).IsAssignableFrom(type);
    }

    private static bool IsGuid(Type type)
    {
        return type == typeof(Guid) || type == typeof(Guid?);
    }

    private static bool IsDateTime(Type type)
    {
        return type == typeof(DateTime) || type == typeof(DateTime?);
    }

    private static bool IsEnumeration(Type type)
    {
        return type.BaseType?.IsGenericType == true &&
               type.BaseType.GetGenericTypeDefinition() == typeof(Enumeration<>);
    }

    private static bool IsCollection(Type type)
    {
        return type.IsGenericType &&
               (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                type.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                type.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>) ||
                type.GetGenericTypeDefinition() == typeof(List<>));
    }
}
