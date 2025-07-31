using AdasIt.Andor.Domain;

namespace AdasIt.Andor.Configurations.Application.Actions;

public interface IAction<T> where T : IId<T>
{
    T Id { get; init; }
}