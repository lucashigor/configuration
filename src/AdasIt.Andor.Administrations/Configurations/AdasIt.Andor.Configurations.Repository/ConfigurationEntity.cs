namespace AdasIt.Andor.Configurations.InfrastructureCommands
{
    public record ConfigurationEntity
    {
        public Guid Id { get; set; }
        public string Name { get; init; } = "";
        public string Value { get; init; } = "";
        public string Description { get; init; } = "";
        public DateTime StartDate { get; init; }
        public DateTime? ExpireDate { get; init; }
        public string CreatedBy { get; init; } = "";
        public DateTime CreatedAt { get; init; }
    }
}
