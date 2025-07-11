using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Errors;
using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Domain;
using System.Collections;

namespace AdasIt.Andor.Configurations.Tests.Domain.Helpers;

public class TestInvalidDataGenerator : IEnumerable<object[]>
{
    public static IEnumerable<object[]> UpdateWithErrorOnExpiredConfig()
    {
        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Expired,
                Name = ConfigurationFixture.GetValidName(),
                Error = ConfigurationsErrorCodes.OnlyDescriptionAllowedToChange,
                FieldName = nameof(Configuration.ExpireDate),
                Because = "Not possible to change Name on expired config"
            }
        };

        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Expired,
                Value = ConfigurationFixture.GetValidValue(),
                Error = ConfigurationsErrorCodes.OnlyDescriptionAllowedToChange,
                FieldName = nameof(Configuration.ExpireDate),
                Because = "Not possible to change Value on expired config"
            }
        };

        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Expired,
                StartDate = ConfigurationFixture.GetValidStartDate(ConfigurationState.Awaiting),
                Error = ConfigurationsErrorCodes.OnlyDescriptionAllowedToChange,
                FieldName = nameof(Configuration.ExpireDate),
                Because = "Not possible to change StartDate on expired config"
            }
        };

        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Expired,
                ExpireDate = ConfigurationFixture.GetValidExpireDate(ConfigurationState.Awaiting),
                Error = ConfigurationsErrorCodes.OnlyDescriptionAllowedToChange,
                FieldName = nameof(Configuration.ExpireDate),
                Because = "Not possible to change ExpireDate on expired config"
            }
        };

        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Active,
                Name = ConfigurationFixture.GetValidName(),
                Error = ConfigurationsErrorCodes.ErrorOnChangeName,
                FieldName = nameof(Configuration.StartDate),
                Because = "Not possible to change Name on Active config"
            }
        };

        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Active,
                StartDate = ConfigurationFixture.GetValidStartDate(ConfigurationState.Awaiting),
                Error = ConfigurationsErrorCodes.ErrorOnChangeName,
                FieldName = nameof(Configuration.StartDate),
                Because = "Not possible to change StartDate on Active config"
            }
        };

        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Active,
                Value = ConfigurationFixture.GetValidValue(),
                Error = ConfigurationsErrorCodes.ErrorOnChangeName,
                FieldName = nameof(Configuration.StartDate),
                Because = "Not possible to change Value on Active config"
            }
        };

        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Awaiting,
                Name = "",
                Error = CommonErrorCodes.Validation,
                FieldName = nameof(Configuration.Name),
                Because = "Not possible to change name to empty on Awaiting config"
            }
        };

        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Awaiting,
                Value = "",
                Error = CommonErrorCodes.Validation,
                FieldName = nameof(Configuration.Value),
                Because = "Not possible to change Value to empty on Awaiting config"
            }
        };

        yield return new object[] {
            new UpdateWithError()
            {
                ConfigurationStatus = ConfigurationState.Awaiting,
                Description = "",
                Error = CommonErrorCodes.Validation,
                FieldName = nameof(Configuration.Description),
                Because = "Not possible to change Description to empty on Awaiting config"
            }
        };
    }

    public static IEnumerable<object[]> ValidConfigurationStatusTestData()
    {
        yield return new object[] { -2, -1, ConfigurationState.Expired };
        yield return new object[] { -2, null!, ConfigurationState.Active };
        yield return new object[] { -2, 1, ConfigurationState.Active };
        yield return new object[] { 1, 2, ConfigurationState.Awaiting };
    }

    public static IEnumerable<object[]> GetPersonFromDataGenerator()
    {
        yield return new object[]
        {
            new BaseConfiguration(
                Name: string.Empty,
                Value: ConfigurationFixture.GetValidValue(),
                Description: ConfigurationFixture.GetValidDescription(),
                StartDate: ConfigurationFixture.GetValidStartDate(ConfigurationState.Awaiting),
                ExpireDate: ConfigurationFixture.GetValidExpireDate(ConfigurationState.Awaiting)),
            CommonErrorCodes.Validation,
            nameof(Configuration.Name)
        };

        yield return new object[]
        {
            new BaseConfiguration(
                Name:ConfigurationFixture.GetValidName(),
                Value: string.Empty,
                Description: ConfigurationFixture.GetValidDescription(),
                StartDate: ConfigurationFixture.GetValidStartDate(ConfigurationState.Awaiting),
                ExpireDate: ConfigurationFixture.GetValidExpireDate(ConfigurationState.Awaiting)),
            CommonErrorCodes.Validation,
            nameof(Configuration.Value)
        };

        yield return new object[]
        {
            new BaseConfiguration(
                Name:ConfigurationFixture.GetValidName(),
                Value: ConfigurationFixture.GetValidValue(),
                Description: string.Empty,
                StartDate: ConfigurationFixture.GetValidStartDate(ConfigurationState.Awaiting),
                ExpireDate: ConfigurationFixture.GetValidExpireDate(ConfigurationState.Awaiting)),
            CommonErrorCodes.Validation,
            nameof(Configuration.Description)
        };

        yield return new object[]
        {
            new BaseConfiguration(
                Name:ConfigurationFixture.GetValidName(),
                Value: ConfigurationFixture.GetValidValue(),
                Description: ConfigurationFixture.GetValidDescription(),
                StartDate: DateTime.UtcNow.AddSeconds(-10),
                ExpireDate: ConfigurationFixture.GetValidExpireDate(ConfigurationState.Awaiting)),
            CommonErrorCodes.Validation,
            nameof(Configuration.StartDate)
        };

        yield return new object[]
        {
            new BaseConfiguration(
                Name:ConfigurationFixture.GetValidName(),
                Value: ConfigurationFixture.GetValidValue(),
                Description: ConfigurationFixture.GetValidDescription(),
                StartDate: ConfigurationFixture.GetValidStartDate(ConfigurationState.Awaiting),
                ExpireDate: ConfigurationFixture.GetValidStartDate(ConfigurationState.Awaiting).AddSeconds(-10)),
            CommonErrorCodes.Validation,
            nameof(Configuration.ExpireDate)
        };

        yield return new object[]
        {
            new BaseConfiguration(
                Name:ConfigurationFixture.GetValidName(),
                Value: ConfigurationFixture.GetValidValue(),
                Description: ConfigurationFixture.GetValidDescription(),
                StartDate: ConfigurationFixture.GetValidStartDate(ConfigurationState.Awaiting),
                ExpireDate: DateTime.UtcNow.AddSeconds(-10)),
            CommonErrorCodes.Validation,
            nameof(Configuration.ExpireDate)
        };
    }

    public IEnumerator<object[]> GetEnumerator() => (IEnumerator<object[]>)GetPersonFromDataGenerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
