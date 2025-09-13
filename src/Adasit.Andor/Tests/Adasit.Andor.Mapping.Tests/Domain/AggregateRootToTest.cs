using AdasIt.Andor.Domain.SeedWork;
using AdasIt.Andor.Domain.ValuesObjects;
using Adasit.Andor.Mapping.Tests.Domain.ValueObject;

namespace Adasit.Andor.Mapping.Tests.Domain;

internal class AggregateRootToTest : AggregateRoot<AggregateRootToTestId>
{
    public Name Name { get; private set; }
    public EntityToTest EntityToTest { get; private set; }
    public DateTime DateTime { get; private set; }
    public DateTime? NullDateTime { get; private set; }
    public string String { get; private set; }
    public string? NullString { get; private set; }
    public Email Email { get; private set; }
    public EnumerationToTest Enum { get; private set; }
    public EntityToTestId? EntityToTestId { get; private set; }

    public IReadOnlyCollection<EntityToTest> EntitiesToTest => [.. _entitiesToTest ?? []];
    private ICollection<EntityToTest> _entitiesToTest { get; set; }

    private AggregateRootToTest()
    {
    }

    private AggregateRootToTest(Name name, EntityToTest entityToTest, DateTime dateTime, DateTime? nullDateTime,
        string @string, string? nullString, Email email, EnumerationToTest @enum)
    {
        Name = name;
        EntityToTest = entityToTest;
        DateTime = dateTime;
        NullDateTime = nullDateTime;
        String = @string;
        NullString = nullString;
        Email = email;
        Enum = @enum;
        _entitiesToTest = [];
    }
}
