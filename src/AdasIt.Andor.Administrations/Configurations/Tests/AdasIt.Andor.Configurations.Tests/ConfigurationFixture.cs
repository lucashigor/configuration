using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.Tests.Domain.Helpers;
using AdasIt.Andor.TestsUtil;

namespace AdasIt.Andor.Configurations.Tests;

public static class ConfigurationFixture
{
    public static Configuration LoadConfiguration(ConfigurationState configurationStatus)
        => LoadConfiguration(GetValidBaseConfiguration(configurationStatus), Guid.NewGuid());

    public static Configuration LoadConfiguration(Guid userId, ConfigurationState configurationStatus)
        => LoadConfiguration(GetValidBaseConfiguration(configurationStatus), userId);

    public static Configuration LoadConfiguration(BaseConfiguration @base)
        => LoadConfiguration(@base, Guid.NewGuid());

    public static Configuration LoadConfiguration(BaseConfiguration @base, Guid userId)
    {
        var propertyValues = new Dictionary<string, object>
        {
            { nameof(Configuration.Name), @base.Name },
            { nameof(Configuration.Value), @base.Value },
            { nameof(Configuration.Description), @base.Description },
            { nameof(Configuration.CreatedBy), userId.ToString() },
            { nameof(Configuration.StartDate), @base.StartDate },
            { nameof(Configuration.CreatedAt), @base.StartDate }
         };

        if (@base.ExpireDate != null)
        {
            propertyValues.Add(nameof(Configuration.ExpireDate), @base.ExpireDate);
        }

        return GeneralFixture.CreateInstanceAndSetProperties<Configuration>(propertyValues);
    }

    public static Configuration GetValidConfiguration()
        => GetValidConfiguration(Guid.NewGuid(), ConfigurationState.Awaiting);

    public static Configuration GetValidConfiguration(ConfigurationState configurationStatus)
        => GetValidConfiguration(Guid.NewGuid(), configurationStatus);

    public static Configuration GetValidConfiguration(Guid userId)
        => GetValidConfiguration(userId, ConfigurationState.Awaiting);

    public static Configuration GetValidConfiguration(Guid userId, ConfigurationState configurationStatus)
    {
        return LoadConfiguration(
            new BaseConfiguration(
            Name: GetValidName(),
            Value: GetValidValue(),
            Description: GetValidDescription(),
            StartDate: GetValidStartDate(configurationStatus),
            ExpireDate: GetValidExpireDate(configurationStatus)), userId);
    }

    public static string GetValidName()
        => GeneralFixture.GetStringRightSize(3, 100);

    public static string GetValidValue()
        => GeneralFixture.GetStringRightSize(1, 2500);

    public static string GetValidDescription()
        => GeneralFixture.GetStringRightSize(3, 1000);

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
            throw new ArgumentOutOfRangeException("Not state mapped");
        }
    }

    public static DateTime GetValidExpireDate(ConfigurationState configuration)
    {
        DateTime currentTime = DateTime.UtcNow;

        if (configuration == ConfigurationState.Awaiting || configuration == ConfigurationState.Active)
        {
            return currentTime.AddMonths(1);
        }
        else if (configuration == ConfigurationState.Expired)
        {
            return currentTime.AddMonths(-1);
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    private static BaseConfiguration GetValidBaseConfiguration(ConfigurationState configurationStatus)
    {
        return new BaseConfiguration(
                        Name: GetValidName(),
                        Value: GetValidValue(),
                        Description: GetValidDescription(),
                        StartDate: GetValidStartDate(configurationStatus),
                        ExpireDate: GetValidExpireDate(configurationStatus)
                    );
    }

}

