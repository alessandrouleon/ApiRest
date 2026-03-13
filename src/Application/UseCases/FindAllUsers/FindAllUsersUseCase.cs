using APIRest.Application.DTOs.Responses;
using APIRest.Application.Mappers;
using APIRest.Domain.Interfaces;

namespace APIRest.Application.UseCases.FindAllUsers;

public class FindAllUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public FindAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponse>> ExecuteAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return UserMapper.ToResponseList(users);
    }
}
