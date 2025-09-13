using Adasit.Andor.Mapping.Tests.Domain;
using Adasit.Andor.Mapping.Tests.Domain.ValueObject;
using Adasit.Andor.Mapping.Tests.Dto;
using AdasIt.Andor.Domain.ValuesObjects;
using AdasIt.Andor.TestsUtil;
using Xunit;

namespace Adasit.Andor.Mapping.Tests;

public class GetValidTests
{
    [Fact]
    public void GetValid_MapsAggregateRootFromDto_Correctly()
    {
        // Arrange
        var entity1 = new EntityToTestDto(Guid.NewGuid(), GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength));
        var entity2 = new EntityToTestDto(Guid.NewGuid(), GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength));

        var dto = new AggregateRootToTestDto
        {
            Name = GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength),
            EntityToTest = new EntityToTestDto(Guid.NewGuid(), GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength)),
            DateTime = DateTime.UtcNow,
            NullDateTime = null,
            String = GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength),
            NullString = null,
            Email = GeneralFixture.Faker.Person.Email,
            Enum = EnumerationToTest.MoneyDeposit.Key,
            _entitiesToTest = new List<EntityToTestDto>
            {
                new EntityToTestDto(Guid.NewGuid(), GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength)),
                new EntityToTestDto(Guid.NewGuid(), GeneralFixture.GetStringRightSize(Name.MinLength, Name.MaxLength))
            }
        };

        // Act
        var result = Mappings.GetValid<AggregateRootToTest, AggregateRootToTestDto>(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name.Value);
        Assert.Equal(dto.EntityToTest.Name, result.EntityToTest.Name.Value);
        Assert.Equal(dto.DateTime, result.DateTime);
        Assert.Equal(dto.NullDateTime, result.NullDateTime);
        Assert.Equal(dto.String, result.String);
        Assert.Equal(dto.NullString, result.NullString);
        Assert.Equal(dto.Email, result.Email.Value);
        Assert.Equal(dto.Enum, result.Enum.Key);

        Assert.Equal(dto._entitiesToTest.Count, result.EntitiesToTest.Count);

        foreach (var dtoEntity in dto._entitiesToTest)
        {
            Assert.Contains(result.EntitiesToTest, e => e.Name.Value == dtoEntity.Name);
        }
    }
}
