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
}

