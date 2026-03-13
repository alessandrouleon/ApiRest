namespace APIRest.Domain.Exceptions;

public class EmailAlreadyInUseException : DomainException
{
    public EmailAlreadyInUseException(string email)
        : base($"The email '{email}' is already in use.") { }
}
