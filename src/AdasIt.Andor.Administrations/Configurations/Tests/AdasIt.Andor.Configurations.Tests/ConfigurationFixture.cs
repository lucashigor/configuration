using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.Tests.Domain.Helpers;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.TestsUtil;
using Mapster;

namespace AdasIt.Andor.Configurations.Tests;

public static class ConfigurationFixture
{
    public static Configuration LoadConfiguration(ConfigurationState configurationStatus)
        => LoadConfiguration(GetValidBaseConfiguration(configurationStatus, Guid.NewGuid()));

    public static Configuration LoadConfiguration(Guid userId, ConfigurationState configurationStatus)
        => LoadConfiguration(GetValidBaseConfiguration(configurationStatus,userId));

    public static Configuration LoadConfiguration(BaseConfiguration @base)
    {
        try
        {
            return @base.Adapt<Configuration>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static string GetValidName()
        => GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength);

    public static string GetValidValue()
        => GeneralFixture.GetStringRightSize(Value.MinLength, Value.MaxLength);

    public static string GetValidDescription()
        => GeneralFixture.GetStringRightSize(Description.MinLength, Description.MaxLength);
    
    private static BaseConfiguration GetValidBaseConfiguration(ConfigurationState configurationStatus, Guid userId)
    {
        return new BaseConfiguration(
            Name: GetValidName(),
            Value: GetValidValue(),
            Description: GetValidDescription(),
            StartDate: GetValidStartDate(configurationStatus),
            ExpireDate: GetValidExpireDate(configurationStatus),
            CreatedBy: userId.ToString(),
            CreatedAt: DateTime.UtcNow
        );
    }

    public static DateTime GetValidStartDate(ConfigurationState state)
    {
        DateTime currentTime = DateTime.UtcNow;

        if (state == ConfigurationState.Awaiting)
        {
            return currentTime.AddDays(1);
        }
        else if (state == ConfigurationState.Active || state == ConfigurationState.Expired)
        {
            return currentTime.AddMonths(-2);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(state), "Not state mapped");
        }
    }

    public static DateTime GetValidExpireDate(ConfigurationState state)
    {
        DateTime currentTime = DateTime.UtcNow;

        if (state == ConfigurationState.Awaiting || state == ConfigurationState.Active)
        {
            return currentTime.AddMonths(1);
        }
        else if (state == ConfigurationState.Expired)
        {
            return currentTime.AddMonths(-1);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(state), "Not state mapped");
        }
    }
}

