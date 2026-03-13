using APIRest.Application.DTOs.Responses;
using APIRest.Domain.Entities;

namespace APIRest.Application.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Username = user.Username,
        Email = user.Email.Value,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    };

    public static IEnumerable<UserResponse> ToResponseList(IEnumerable<User> users)
        => users.Select(ToResponse);
}
