namespace AdasIt.Andor.Configurations.DomainQueries;

public record ConfigurationState(int Key, string Value)
{
    public static readonly ConfigurationState Undefined = new(0, nameof(Undefined));
    public static readonly ConfigurationState Awaiting = new(1, nameof(Awaiting));
    public static readonly ConfigurationState Active = new(2, nameof(Active));
    public static readonly ConfigurationState Expired = new(3, nameof(Expired));
    public static readonly ConfigurationState Deleted = new(4, nameof(Deleted));
}