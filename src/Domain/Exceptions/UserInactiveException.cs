namespace APIRest.Domain.Exceptions;

public class UserInactiveException : DomainException
{
    public UserInactiveException(Guid id)
        : base($"User with id '{id}' is inactive and cannot be modified.") { }
}
