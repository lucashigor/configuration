using AdasIt.Andor.Domain;

namespace AdasIt.Andor.Budgets.Domain.Accounts;

public record AccountStatus : Enumeration<int>
{
    private AccountStatus(int key, string name) : base(key, name) { }

    public static readonly AccountStatus Undefined = new(0, "undefined");
    public static readonly AccountStatus Active = new(1, nameof(Active));
    public static readonly AccountStatus Inactive = new(1, nameof(Inactive));
}
