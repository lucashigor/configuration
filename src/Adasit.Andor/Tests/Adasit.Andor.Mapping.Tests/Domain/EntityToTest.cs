using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;
using Adasit.Andor.Mapping.Tests.Domain.ValueObject;

namespace Adasit.Andor.Mapping.Tests.Domain;

internal class EntityToTest : Entity<EntityToTestId>
{
    public Name Name { get; private set; }

    private EntityToTest()
    {

    }

    private EntityToTest(string name)
    {
        Name = name;
    }
}
