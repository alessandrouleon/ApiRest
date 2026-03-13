namespace APIRest.Domain.Exceptions;

public class UserNotFoundException : DomainException
{
    public UserNotFoundException(Guid id)
        : base($"User with id '{id}' was not found.") { }
}
