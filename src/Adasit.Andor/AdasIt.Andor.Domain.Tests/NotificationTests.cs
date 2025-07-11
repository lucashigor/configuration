using AdasIt.Andor.Domain.ValuesObjects;

namespace AdasIt.Andor.Domain.Tests;

public class NotificationTests
{
    [Fact]
    public void ToString_ReturnsExpectedString()
    {
        var fieldName = "FieldName";
        var errorMessage = "Error Message";
        var errorCode = CommonErrorCodes.Validation;

        // Arrange
        var notification = new Notification(fieldName, errorMessage,
            errorCode);

        // Act
        var result = notification.ToString();

        // Assert

        var expected = $"Field: {fieldName} - Error: {errorCode}: Message - {errorMessage}";

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_WithFieldName_Message_Error_SetsPropertiesCorrectly()
    {
        // Arrange
        var fieldName = "Email";
        var message = "Email is invalid";
        var error = CommonErrorCodes.Validation;

        // Act
        var notification = new Notification(fieldName, message, error);

        // Assert
        Assert.Equal(fieldName, notification.FieldName);
        Assert.Equal(message, notification.Message);
        Assert.Equal(error, notification.Error);
    }

    [Fact]
    public void Constructor_WithMessageAndErrorOnly_SetsFieldNameAsEmpty()
    {
        // Arrange
        var message = "A general error occurred";
        var error = CommonErrorCodes.General;

        // Act
        var notification = new Notification(message, error);

        // Assert
        Assert.Equal(string.Empty, notification.FieldName);
        Assert.Equal(message, notification.Message);
        Assert.Equal(error, notification.Error);
    }

    [Fact]
    public void ToString_WithEmptyFieldName_ReturnsExpectedString()
    {
        // Arrange
        var message = "Something went wrong";
        var error = CommonErrorCodes.Internal;

        var notification = new Notification(message, error);

        // Act
        var result = notification.ToString();

        // Assert
        var expected = $"Field:  - Error: {error}: Message - {message}";

        Assert.Equal(expected, result);
    }
}

