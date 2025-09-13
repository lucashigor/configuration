using Adasit.Andor.Mapping.Tests.Domain;
using Adasit.Andor.Mapping.Tests.Domain.ValueObject;
using Adasit.Andor.Mapping.Tests.Dto;
using AdasIt.Andor.Domain;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.TestsUtil;
using Xunit;

namespace Adasit.Andor.Mapping.Tests;

public class CreateInstanceAndSetPropertiesTests
{
    [Fact]
    public void CreateInstance_SetsStringProperty_Correctly()
    {
        var value = GeneralFixture.GetStringRightSize(20, 30);

        var fields = new Dictionary<string, object?>
        {
            { nameof(AggregateRootToTest.String), value }
        };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Equal(value, result.String);
    }

    [Fact]
    public void CreateInstance_SetsEnumerationProperty_Correctly()
    {
        var allEnums = Enumeration<int>.GetAll<EnumerationToTest>().ToList();

        var random = new Random();
        var expectedEnum = allEnums[random.Next(allEnums.Count)];

        var fields = new Dictionary<string, object?>
        {
            { nameof(AggregateRootToTest.Enum), expectedEnum.Key }
        };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Equal(expectedEnum.Key, result.Enum.Key);
        Assert.Equal(expectedEnum.Name, result.Enum.Name);
    }

    [Fact]
    public void CreateInstance_SetsMailAddressProperty_Correctly()
    {
        var value = GeneralFixture.Faker.Person.Email;

        var fields = new Dictionary<string, object?>
        {
            { nameof(AggregateRootToTest.Email), value }
        };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Equal(value, result.Email.Value);
    }

    [Fact]
    public void CreateInstance_SetsDateTimeProperty_Correctly()
    {
        var value = GeneralFixture.Faker.Date.Future();

        var fields = new Dictionary<string, object?>
        {
            { nameof(AggregateRootToTest.DateTime), value }
        };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Equal(value, result.DateTime);
    }

    [Fact]
    public void CreateInstance_SetsNullableDateTime_Correctly()
    {
        var value = GeneralFixture.Faker.Date.Future();

        var fields = new Dictionary<string, object?>
    {
        { nameof(AggregateRootToTest.NullDateTime), value }
    };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Equal(value, result.NullDateTime);
    }

    [Fact]
    public void CreateInstance_SetsNullableDateTime_ToNull()
    {
        var fields = new Dictionary<string, object?>
    {
        { nameof(AggregateRootToTest.NullDateTime), null }
    };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Null(result.NullDateTime);
    }

    [Fact]
    public void CreateInstance_SetsIdProperty_Correctly()
    {
        var value = Guid.NewGuid();

        var fields = new Dictionary<string, object?>
        {
            { nameof(AggregateRootToTest.Id), value }
        };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Equal(value, result.Id.Value);
    }

    [Fact]
    public void CreateInstance_SetsStringValueObject_Correctly()
    {
        var value = GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength);

        var fields = new Dictionary<string, object?>
        {
            { nameof(AggregateRootToTest.Name), value }
        };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Equal(value, result.Name.Value);
    }

    [Fact]
    public void CreateInstance_SetsNestedEntity_Correctly()
    {
        var nested = new EntityToTestDto(Guid.NewGuid(),
            GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength));

        var fields = new Dictionary<string, object?>
        {
            { nameof(AggregateRootToTest.EntityToTest), nested }
        };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Equal(nested.Name, result.EntityToTest.Name);
        Assert.Equal(nested.Id, result.EntityToTest.Id.Value);
    }

    [Fact]
    public void CreateInstance_SetsCollectionProperty_Correctly()
    {
        var entity1 = new EntityToTestDto(Guid.NewGuid(), "Entity 1");
        var entity2 = new EntityToTestDto(Guid.NewGuid(), "Entity 2");

        var fields = new Dictionary<string, object?>
        {
            { "_entitiesToTest", new List<EntityToTestDto> { entity1, entity2 } }
        };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);
        Assert.Equal(2, result.EntitiesToTest.Count);
        Assert.Contains(result.EntitiesToTest, e => (Guid)e.Id == entity1.Id && e.Name == entity1.Name);
        Assert.Contains(result.EntitiesToTest, e => (Guid)e.Id == entity2.Id && e.Name == entity2.Name);
    }

    [Fact]
    public void CreateInstance_SetsEntityToTestIdProperty_Correctly()
    {
        Guid? id = null;

        var fields = new Dictionary<string, object?>
        {
            { nameof(AggregateRootToTest.EntityToTestId), id }
        };

        var result = Mappings.CreateInstanceAndSetProperties<AggregateRootToTest>(fields);

        Assert.NotNull(result);

        Assert.Null(result.EntityToTestId);
    }

}
