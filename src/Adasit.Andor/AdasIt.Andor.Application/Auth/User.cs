using AdasIt.Andor.Domain;

namespace AdasIt.Andor.Application.Auth;

public class User : IUser
{
    public Guid Id => Guid.NewGuid();
}
