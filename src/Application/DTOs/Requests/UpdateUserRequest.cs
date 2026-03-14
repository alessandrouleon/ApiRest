using APIRest.Domain.Enums;

namespace APIRest.Application.DTOs.Requests;

public class UpdateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public UserRole Role { get; set; }
    public string? Password { get; set; }
}
