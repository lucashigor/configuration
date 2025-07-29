namespace AdasIt.Andor.InfrastructureQueries
{
    public record ProcessedEvents(
        Guid AggregatorId,
        string ProjectionName,
        Guid EventId,
        DateTime ProcessedAt
        );
}
