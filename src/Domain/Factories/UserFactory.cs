using APIRest.Domain.Entities;
using APIRest.Domain.ValueObjects;

namespace APIRest.Domain.Factories;

public static class UserFactory
{
    public static User Create(string name, string email, string passwordHash)
    {
        return new User(
            id: Guid.NewGuid(),
            name: name,
            email: new Email(email),
            passwordHash: passwordHash,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow
        );
    }
}
