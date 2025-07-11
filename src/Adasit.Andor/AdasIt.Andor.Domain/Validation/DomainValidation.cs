using AdasIt.Andor.Domain.ValuesObjects;
using System;
using System.Runtime.CompilerServices;

namespace AdasIt.Andor.Domain.Validation;

public static class DomainValidation
{
    public static Notification? NotNull(this object target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (target is null)
        {
            var message = DefaultsErrorsMessages.NotNull.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    public static Notification? NotNull(this Guid target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (target == Guid.Empty)
        {
            var message = DefaultsErrorsMessages.NotNull.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    public static Notification? NotNullOrEmptyOrWhiteSpace(this string? target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrEmpty(target))
        {
            var message = DefaultsErrorsMessages.NotNull.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    public static Notification? NotDefaultDateTime(this DateTime target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        DateTime? nullableTarget = target;

        var notification = nullableTarget.NotDefaultDateTime(fieldName);

        return notification;
    }

    public static Notification? NotDefaultDateTime(this DateTime? target,
                          [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (target.HasValue && target.Value == default)
        {
            var message = DefaultsErrorsMessages.NotDefaultDateTime.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    public static Notification? BetweenLength(this string? target, int minLength, int maxLength,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (!string.IsNullOrEmpty(target) && (target.Length < minLength || target.Length > maxLength))
        {
            var message = DefaultsErrorsMessages.BetweenLength.GetMessage(fieldName, minLength, maxLength);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    public static Notification? ValidUrl(this string target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (!string.IsNullOrEmpty(target) && !Uri.TryCreate(target, UriKind.Absolute, out _))
        {
            var message = DefaultsErrorsMessages.InvalidUrl.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }
}
