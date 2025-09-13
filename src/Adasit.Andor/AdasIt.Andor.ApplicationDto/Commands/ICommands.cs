namespace AdasIt.Andor.ApplicationDto.Commands;

public interface ICommands<T>
{
    T Id { get; init; }
}
