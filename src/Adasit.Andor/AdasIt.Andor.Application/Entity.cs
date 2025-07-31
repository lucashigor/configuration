namespace AdasIt.Andor.DomainQueries;

public record Entity<TEntityKey> where TEntityKey : IEquatable<TEntityKey>
{
    public required TEntityKey Id { get; init; }
}
