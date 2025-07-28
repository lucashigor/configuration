namespace AdasIt.Andor.Infrastructure
{
    public record ProcessedEvents(
        Guid AggregatorId,
        string ProjectionName,
        Guid EventId,
        DateTime ProcessedAt
        );
}
