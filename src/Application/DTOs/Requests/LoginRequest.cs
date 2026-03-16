namespace APIRest.Application.DTOs.Requests;

public record LoginRequest(
    string Identifier, // email or username
    string Password
);
