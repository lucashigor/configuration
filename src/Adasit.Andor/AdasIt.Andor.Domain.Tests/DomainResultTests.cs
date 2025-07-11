using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Domain.Tests;

public class DomainResultTests
{
    [Fact]
    public void Success_WithoutWarningsOrInfos_ReturnsSuccessResult()
    {
        // Act
        var result = DomainResult.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Errors);
        Assert.Empty(result.Warnings);
        Assert.Empty(result.Infos);
    }

    [Fact]
    public void Success_WithWarningsAndInfos_ReturnsSuccessResultWithCollections()
    {
        // Arrange
        var warnings = new List<Notification>
        {
            new("WarningField", "This is a warning", CommonErrorCodes.InvalidYear)
        };
        var infos = new List<Notification>
        {
            new("InfoField", "This is info", CommonErrorCodes.InvalidMonth)
        };

        // Act
        var result = DomainResult.Success(warnings, infos);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Errors);
        Assert.Equal(warnings, result.Warnings);
        Assert.Equal(infos, result.Infos);
    }

    [Fact]
    public void Failure_WithErrorsOnly_ReturnsFailureResult()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new("ErrorField", "Something failed", CommonErrorCodes.Validation)
        };

        // Act
        var result = DomainResult.Failure(errors);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(errors, result.Errors);
        Assert.Empty(result.Warnings);
        Assert.Empty(result.Infos);
    }

    [Fact]
    public void Failure_WithAllCollections_ReturnsFailureResultWithAll()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new("ErrorField", "Invalid input", CommonErrorCodes.Validation)
        };
        var warnings = new List<Notification>
        {
            new("WarningField", "Deprecated field", CommonErrorCodes.InvalidYear)
        };
        var infos = new List<Notification>
        {
            new("InfoField", "Processing info", CommonErrorCodes.InvalidMonth)
        };

        // Act
        var result = DomainResult.Failure(errors, warnings, infos);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(errors, result.Errors);
        Assert.Equal(warnings, result.Warnings);
        Assert.Equal(infos, result.Infos);
    }

    [Fact]
    public void Failure_WithEmptyErrors_ShouldTrhowException()
    {
        var ex = Assert.Throws<ArgumentException>(() => DomainResult.Failure(new List<Notification>()));
        Assert.Equal("Failure must contain at least one error notification.", ex.Message);
    }

    [Fact]
    public void Failure_WithNullErrors_ShouldTrhowException()
    {
        var ex = Assert.Throws<ArgumentException>(() => DomainResult.Failure(null));
        Assert.Equal("Failure must contain at least one error notification.", ex.Message);
    }
}
