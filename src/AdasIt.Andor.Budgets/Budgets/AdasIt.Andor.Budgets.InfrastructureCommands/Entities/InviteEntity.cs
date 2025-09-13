namespace AdasIt.Andor.Budgets.InfrastructureCommands.Entities;

internal record InviteEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public Guid InvitingId { get; set; }
    public UserEntity Inviting { get; set; }
    public Guid? GuestId { get; set; }
    public UserEntity Guest { get; set; }
    public int Status { get; set; }
}
