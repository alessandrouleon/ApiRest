using APIRest.Domain.Entities;
using APIRest.Domain.Enums;
using APIRest.Domain.ValueObjects;

namespace APIRest.Domain.Factories;

public static class UserFactory
{
    public static User Create(string name, string username, string email, bool isActive, UserRole role, string passwordHash)
    {
        return new User(
            id: Guid.NewGuid(),
            name: name,
            username: username,
            email: new Email(email),
            isActive: isActive,
            role: role,
            passwordHash: passwordHash,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow
        );
    }
}
