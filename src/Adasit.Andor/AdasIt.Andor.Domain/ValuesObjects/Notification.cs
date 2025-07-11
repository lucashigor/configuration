namespace AdasIt.Andor.Domain.ValuesObjects;

public sealed record Notification
{
    public DomainErrorCode Error { get; init; }
    public string FieldName { get; init; }
    public string Message { get; init; }

    public Notification(string fieldName, string message, DomainErrorCode error)
    {
        Error = error;
        FieldName = fieldName;
        Message = message;
    }
    public Notification(string message, DomainErrorCode error)
    {
        Error = error;
        FieldName = string.Empty;
        Message = message;
    }

    public override string ToString() => $"Field: {FieldName} - Error: {Error.Value}: Message - {Message}";
}
