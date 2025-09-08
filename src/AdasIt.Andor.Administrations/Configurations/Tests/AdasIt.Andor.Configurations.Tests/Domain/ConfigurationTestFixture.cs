using AdasIt.Andor.Configurations.Domain.ValueObjects;
using AdasIt.Andor.Configurations.Tests.Domain.Helpers;

namespace AdasIt.Andor.Configurations.Tests.Domain;

public class ConfigurationTestFixture
{
    [CollectionDefinition(nameof(ConfigurationTestFixture))]
    public class ConfigurationTestFixtureCollection : ICollectionFixture<ConfigurationTestFixture>
    {
    }
}
