namespace APIRest.Domain.Exceptions;

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException()
        : base("Invalid credentials.") { }
}
