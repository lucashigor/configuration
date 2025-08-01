using Adasit.Andor.Auth;
using AdasIt.Andor.OpenTelemetry;

namespace AdasIt.Andor.WebApi;

public record ApplicationSettings
{
    public Cors? Cors { get; init; }
    public IdentityProvider? IdentityProvider { get; init; }
    public OpenTelemetryConfig? OpenTelemetryConfig { get; init; }
}


public record Cors(List<string>? AllowedOrigins);
