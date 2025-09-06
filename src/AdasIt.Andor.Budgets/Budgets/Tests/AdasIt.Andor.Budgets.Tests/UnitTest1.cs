using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Domain.ValuesObjects;
using NSubstitute;

namespace AdasIt.Andor.Budgets.Tests;

public class SubCategoryTests
{
    private Category CreateCategory() =>
        Category.NewAsync(new("TestCategory", "TestDescription", DateTime.UtcNow, DateTime.UtcNow.AddYears(1), null, null);

    private PaymentMethod CreatePaymentMethod() =>
        new("TestPayment", "TestDescription", DateTime.UtcNow, DateTime.UtcNow.AddYears(1), null, null);

    [Fact]
    public async Task NewAsync_ReturnsSuccess_WhenValidationPasses()
    {
        // Arrange
        var validator = Substitute.For<ISubCategoryValidator>();
        validator.ValidateCreationAsync(
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(),
            Arg.Any<Category>(), Arg.Any<PaymentMethod>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new List<Notification>()));

        var category = CreateCategory();
        var paymentMethod = CreatePaymentMethod();

        // Act
        var (result, subCategory) = await SubCategory.NewAsync(
            "ValidName", "ValidDescription", DateTime.UtcNow, DateTime.UtcNow.AddYears(1),
            category, paymentMethod, validator, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(subCategory);
        Assert.Equal("ValidName", subCategory.Name);
    }

    [Fact]
    public async Task NewAsync_ReturnsFailure_WhenValidatorReturnsErrors()
    {
        // Arrange
        var validator = Substitute.For<ISubCategoryValidator>();
        var notifications = new List<Notification>
        {
            new Notification("Name", "Name is required", DomainErrorCode.New(1))
        };
        validator.ValidateCreationAsync(
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(),
            Arg.Any<Category>(), Arg.Any<PaymentMethod>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(notifications));

        var category = CreateCategory();
        var paymentMethod = CreatePaymentMethod();

        // Act
        var (result, subCategory) = await SubCategory.NewAsync(
            "", "ValidDescription", DateTime.UtcNow, DateTime.UtcNow.AddYears(1),
            category, paymentMethod, validator, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Null(subCategory);
        Assert.Contains(result.Errors, n => n.FieldName == "Name");
    }

    [Fact]
    public async Task NewAsync_ReturnsFailure_WhenEntityValidationFails()
    {
        // Arrange
        var validator = Substitute.For<ISubCategoryValidator>();
        validator.ValidateCreationAsync(
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(),
            Arg.Any<Category>(), Arg.Any<PaymentMethod>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new List<Notification>()));

        var category = CreateCategory();
        var paymentMethod = CreatePaymentMethod();

        // Act
        var (result, subCategory) = await SubCategory.NewAsync(
            null, "ValidDescription", DateTime.UtcNow, DateTime.UtcNow.AddYears(1),
            category, paymentMethod, validator, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Null(subCategory);
    }

    [Fact]
    public async Task NewAsync_CallsValidatorWithCorrectParameters()
    {
        // Arrange
        var validator = Substitute.For<ISubCategoryValidator>();
        validator.ValidateCreationAsync(
            "TestName", "TestDesc", Arg.Any<DateTime>(), Arg.Any<DateTime>(),
            Arg.Any<Category>(), Arg.Any<PaymentMethod>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new List<Notification>()));

        var category = CreateCategory();
        var paymentMethod = CreatePaymentMethod();

        // Act
        await SubCategory.NewAsync(
            "TestName", "TestDesc", DateTime.UtcNow, DateTime.UtcNow.AddYears(1),
            category, paymentMethod, validator, CancellationToken.None);

        // Assert
        await validator.Received(1).ValidateCreationAsync(
            "TestName", "TestDesc", Arg.Any<DateTime>(), Arg.Any<DateTime>(),
            category, paymentMethod, Arg.Any<CancellationToken>());
    }
}