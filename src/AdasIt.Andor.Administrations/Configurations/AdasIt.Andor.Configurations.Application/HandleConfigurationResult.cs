using AdasIt.Andor.ApplicationDto.Results;
using AdasIt.Andor.Configurations.ApplicationDto;
using AdasIt.Andor.Configurations.Domain.Errors;
using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Configurations.Application;

public static class HandleConfigurationResult
{
    private static readonly Dictionary<DomainErrorCode, ErrorModel> _errorsMapping = new()
        {
            { ConfigurationsErrorCodes.ErrorOnChangeName, ConfigurationErrors.ConfigurationValidation()}
        };

    public static async Task HandleResultConfiguration<T>(DomainResult result,
        ApplicationResult<T> notifier) where T : class
    {
        await HandleResult.Handle<T>(result, notifier, _errorsMapping);
    }
}
