namespace AdasIt.Andor.DomainQueries.ResearchableRepository;

public record SearchOutput<TAggregate>(
    int CurrentPage,
    int PerPage,
    int Total,
    ICollection<TAggregate> Items
);
