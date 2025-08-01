namespace AdasIt.Andor.OpenTelemetry;

public record OpenTelemetryConfig
{
    public string? StatusGaugeName { get; init; }
    public string? DurationGaugeName { get; init; }
    public string? Endpoint { get; init; }
    public string? ApplicationInsights { get; init; }
}
