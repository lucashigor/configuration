using AdasIt.Andor.ApplicationDto.Commands;

namespace AdasIt.Andor.Budgets.ApplicationDto;

public record CreateAccount : ICommands<Guid>
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; init; }
    public required string Description { get; init; }
    public CancellationToken CancellationToken { get; init; }
}
