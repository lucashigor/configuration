namespace AdasIt.Andor.DomainQueries;

public interface ISoftDeletableEntity
{
    bool IsDeleted { get; }
}
