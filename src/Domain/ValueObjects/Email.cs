using System.Text.RegularExpressions;

namespace APIRest.Domain.ValueObjects;

public class Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; private set; } = string.Empty;

    private Email() { }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException($"'{value}' is not a valid email address.");

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is Email other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public static implicit operator string(Email email) => email.Value;
}
