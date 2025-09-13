namespace AdasIt.Andor.Budgets.InfrastructureCommands.Entities;

internal record AccountEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public CurrencyEntity Currency { get; set; }
    public int Status { get; set; }

    public ICollection<CategoryEntity> _categories { get; set; }

    public ICollection<SubCategoryEntity> _subCategories { get; set; }

    public ICollection<PaymentMethodEntity> _paymentMethods { get; set; }

    public ICollection<UserEntity> _members { get; set; }

    public ICollection<InviteEntity> _invites { get; set; }
}
