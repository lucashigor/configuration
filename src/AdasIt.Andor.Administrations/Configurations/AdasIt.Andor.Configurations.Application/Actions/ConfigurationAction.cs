using AdasIt.Andor.Configurations.Domain.ValueObjects;

namespace AdasIt.Andor.Configurations.Application.Actions;

public abstract record ConfigurationAction(
    ConfigurationId Id,
    string Name,
    string Value,
    string Description,
    DateTime StartDate,
    DateTime? ExpireDate,
    CancellationToken CancellationToken) : IAction<ConfigurationId>;