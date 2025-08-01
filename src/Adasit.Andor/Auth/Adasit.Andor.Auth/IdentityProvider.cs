namespace Adasit.Andor.Auth;

public record IdentityProvider
{
    public List<string>? Scopes { get; init; }
    public string? SecretKey { get; init; }
    public string? Authority { get; init; }
    public string? SwaggerClientId { get; init; }
    public string? PublicKeyJwt { get; set; }
}
