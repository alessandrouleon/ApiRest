using APIRest.Domain.Enums;
using APIRest.Domain.ValueObjects;

namespace APIRest.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    public UserRole Role { get; private set; } = UserRole.Operator;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private User() { }

    public User(Guid id, string name, string username, Email email, bool isActive, UserRole role, string passwordHash, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Username = username;
        Email = email;
        IsActive = isActive;
        Role = role;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void Update(string name, string username, Email email, bool isActive, UserRole role)
    {
        Name = name;
        Username = username;
        Email = email;
        IsActive = isActive;
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
