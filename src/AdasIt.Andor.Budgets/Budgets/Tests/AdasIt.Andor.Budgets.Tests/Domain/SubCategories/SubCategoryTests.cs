using AdasIt.Andor.Budgets.Domain.Categories;
using AdasIt.Andor.Budgets.Domain.PaymentMethods;
using AdasIt.Andor.Budgets.Domain.SubCategories;
using AdasIt.Andor.Budgets.Tests.Domain;
using AdasIt.Andor.Domain.ValuesObjects;
using NSubstitute;

namespace AdasIt.Andor.Budgets.Tests;

public class SubCategoryTests
{
    private static Category CreateCategory() =>
        CategoriesFixture.GetValidDepositCategory();

    private static PaymentMethod CreatePaymentMethod() =>
        PaymentMethodFixture.GetValidDepositPaymentMethod();

    [Fact]
    public async Task NewAsync_ReturnsSuccess_WhenValidationPasses()
    {
        // Arrange
        var validator = Substitute.For<ISubCategoryValidator>();
        validator.ValidateCreationAsync(
            Arg.Any<SubCategory>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new List<Notification>()));

        var category = CreateCategory();
        var paymentMethod = CreatePaymentMethod();

        // Act
        var (result, subCategory) = await SubCategory.NewAsync(
            "ValidName", "ValidDescription", DateTime.UtcNow, DateTime.UtcNow.AddYears(1),
            category.Id, paymentMethod.Id, category.Type, validator, CancellationToken.None);

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
                Arg.Any<SubCategory>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(notifications));

        var category = CreateCategory();
        var paymentMethod = CreatePaymentMethod();

        // Act
        var (result, subCategory) = await SubCategory.NewAsync(
            "", "ValidDescription", DateTime.UtcNow, DateTime.UtcNow.AddYears(1),
            category.Id, paymentMethod.Id, category.Type, validator, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Null(subCategory);
        Assert.Contains(result.Errors, n => n.FieldName == "Name");
    }
}