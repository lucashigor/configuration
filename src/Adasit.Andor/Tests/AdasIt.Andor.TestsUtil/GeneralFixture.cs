using Bogus;

namespace AdasIt.Andor.TestsUtil;

public static class GeneralFixture
{
    public static Faker Faker { get; set; } = new Faker();

    public static string GetStringRightSize(int minLength, int maxLength)
    {
        var stringValue = Faker.Lorem.Random.Words(2);

        while (stringValue.Length < minLength)
        {
            stringValue += Faker.Lorem.Random.Words(2);
        }

        if (stringValue.Length > maxLength)
        {
            stringValue = stringValue[..maxLength];
        }

        return stringValue;
    }

    public static T CreateInstanceAndSetProperties<T>(Dictionary<string, object> propertyValues) where T : class
    {
        Type type = typeof(T);

        var instance = (T)Activator.CreateInstance(type, true);

        foreach (var property in typeof(T).GetProperties())
        {
            if (propertyValues.TryGetValue(property.Name, out var value))
            {
                property.SetValue(instance, value);
            }
        }

        return instance;
    }

}

