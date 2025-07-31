namespace AdasIt.Andor.Domain;

public interface IId<out TSelf> where TSelf : IId<TSelf>
{
    static abstract TSelf New();
}
