using AdasIt.Andor.Budgets.Domain.Users.ValueObjects;
using AdasIt.Andor.Domain.Validation;

namespace AdasIt.Andor.Budgets.Domain.Users;

public interface IUserValidator : IDefaultValidator<User, UserId>
{
}
