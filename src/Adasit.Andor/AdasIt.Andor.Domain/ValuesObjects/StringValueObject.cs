using System;

namespace AdasIt.Andor.Domain.ValuesObjects;

public abstract record StringValueObject
{
    public string Value { get; }

    protected StringValueObject(string value, int minLength, int maxLength, string name)
    {
        value = value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{name} cannot be empty", nameof(value));

        if (value.Length < minLength || value.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(value),
                $"{name} must be between {minLength} and {maxLength} characters.");

        Value = value;
    }

    public override string ToString() => Value;
}