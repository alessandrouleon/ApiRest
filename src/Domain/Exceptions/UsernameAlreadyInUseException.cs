namespace APIRest.Domain.Exceptions;

public class UsernameAlreadyInUseException : DomainException
{
    public UsernameAlreadyInUseException(string username)
        : base($"The username '{username}' is already in use.") { }
}
