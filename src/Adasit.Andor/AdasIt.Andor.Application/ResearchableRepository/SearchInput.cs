namespace AdasIt.Andor.DomainQueries.ResearchableRepository;

public record SearchInput
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string? Search { get; set; }
    public string? OrderBy { get; set; }
    public SearchOrder Order { get; set; }
}
