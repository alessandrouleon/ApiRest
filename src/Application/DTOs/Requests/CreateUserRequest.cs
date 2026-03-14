using System.ComponentModel;
using APIRest.Domain.Enums;

namespace APIRest.Application.DTOs.Requests;

public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    [DefaultValue(UserRole.Operator)]
    public UserRole Role { get; set; } = UserRole.Operator;
    public string Password { get; set; } = string.Empty;
}
