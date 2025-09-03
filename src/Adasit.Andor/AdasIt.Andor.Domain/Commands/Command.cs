using System;

namespace AdasIt.Andor.Domain.Commands;

public abstract record Command
{
    public DateTime CommandDate { get; init; } = DateTime.UtcNow;
    public required Guid UserId { get; init; }
}
