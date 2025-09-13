using AdasIt.Andor.Domain.ValuesObjects;
using Bogus;

namespace AdasIt.Andor.TestsUtil;

public static class GeneralFixture
{
    public static Faker Faker { get; set; } = new Faker();

    public static string GetStringRightSize(int minLength, int maxLength)
    {
        var stringValue = Faker.Lorem.Random.Words(1);

        while (stringValue.Length < minLength)
        {
            stringValue += Faker.Lorem.Random.Words(1);
        }

        if (stringValue.Length > maxLength)
        {
            stringValue = stringValue[..maxLength];
        }

        return stringValue;
    }

    public static T CreateInstanceAndSetProperties<T>(Dictionary<string, object> propertyValues) where T : class
    {
        var type = typeof(T);

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

    public static Email GetValidEmail()
        => Faker.Person.Email;


    public static Name GetValidName()
        => GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength);

    public static Description GetValidDescription()
        => GeneralFixture.GetStringRightSize(Description.MinLength, Description.MaxLength);
}

