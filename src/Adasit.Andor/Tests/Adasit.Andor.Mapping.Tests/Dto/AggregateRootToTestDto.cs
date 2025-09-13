namespace Adasit.Andor.Mapping.Tests.Dto;

internal class AggregateRootToTestDto
{
    public string Name { get; set; }
    public EntityToTestDto EntityToTest { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? NullDateTime { get; set; }
    public string String { get; set; }
    public string? NullString { get; set; }
    public string Email { get; set; }
    public int Enum { get; set; }

    public ICollection<EntityToTestDto> _entitiesToTest { get; set; }
}
