namespace AdasIt.Andor.Configurations.InfrastructureCommands
{
    public record ConfigurationEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
