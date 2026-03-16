# Estrutura do Projeto

Legenda dos arquivos em stage (relacionados a **Autenticação e Autorização**):

| Marcador | Significado |
|----------|-------------|
| `🆕 NEW` | Arquivo novo adicionado ao stage |
| `✏️ MOD` | Arquivo existente modificado no stage |

---

```
src/
├── API/
│   ├── Common/
│   │   ├── ApiResponse.cs
│   │   ├── DefaultValueEnumSchemaFilter.cs
│   │   └── Roles.cs                                        🆕 NEW
│   ├── Controllers/
│   │   ├── AuthController.cs                               🆕 NEW
│   │   └── UsersController.cs                             ✏️ MOD
│   ├── Middlewares/
│   │   └── GlobalExceptionMiddleware.cs                   ✏️ MOD
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── API.csproj                                         ✏️ MOD
│   ├── Program.cs                                         ✏️ MOD
│   ├── appsettings.Development.json                       ✏️ MOD
│   └── appsettings.json                                   ✏️ MOD
│
├── Application/
│   ├── DTOs/
│   │   ├── Requests/
│   │   │   ├── CreateUserRequest.cs
│   │   │   ├── LoginRequest.cs                             🆕 NEW
│   │   │   ├── RefreshTokenRequest.cs                      🆕 NEW
│   │   │   └── UpdateUserRequest.cs
│   │   └── Responses/
│   │       ├── AuthResponse.cs                             🆕 NEW
│   │       └── UserResponse.cs
│   ├── Interfaces/
│   │   ├── IPasswordHasher.cs
│   │   └── ITokenService.cs                               🆕 NEW
│   ├── Mappers/
│   │   └── UserMapper.cs
│   ├── UseCases/
│   │   ├── Auth/
│   │   │   ├── LoginUseCase.cs                             🆕 NEW
│   │   │   ├── RefreshTokenUseCase.cs                      🆕 NEW
│   │   │   └── RevokeTokenUseCase.cs                       🆕 NEW
│   │   ├── CreateUser/
│   │   │   └── CreateUserUseCase.cs
│   │   ├── DeleteUser/
│   │   │   └── DeleteUserUseCase.cs
│   │   ├── FindAllUsers/
│   │   │   └── FindAllUsersUseCase.cs
│   │   ├── FindUserById/
│   │   │   └── FindUserByIdUseCase.cs
│   │   └── UpdateUser/
│   │       └── UpdateUserUseCase.cs
│   ├── Validators/
│   │   ├── CreateUserRequestValidator.cs
│   │   ├── LoginRequestValidator.cs                        🆕 NEW
│   │   └── UpdateUserRequestValidator.cs
│   └── Application.csproj
│
├── Domain/
│   ├── Entities/
│   │   ├── RefreshToken.cs                                 🆕 NEW
│   │   └── User.cs
│   ├── Enums/
│   │   └── UserRole.cs
│   ├── Exceptions/
│   │   ├── DomainException.cs
│   │   ├── EmailAlreadyInUseException.cs
│   │   ├── InvalidCredentialsException.cs                  🆕 NEW
│   │   ├── UserInactiveException.cs
│   │   ├── UserNotFoundException.cs
│   │   └── UsernameAlreadyInUseException.cs
│   ├── Factories/
│   │   └── UserFactory.cs
│   ├── Interfaces/
│   │   ├── IRefreshTokenRepository.cs                      🆕 NEW
│   │   └── IUserRepository.cs
│   ├── ValueObjects/
│   │   └── Email.cs
│   └── Domain.csproj
│
└── Infrastructure/
    ├── Data/
    │   ├── Configurations/
    │   │   ├── RefreshTokenConfiguration.cs                🆕 NEW
    │   │   └── UserConfiguration.cs
    │   └── AppDbContext.cs                                 ✏️ MOD
    ├── Migrations/
    │   ├── 20260314011111_CreateUser.cs
    │   ├── 20260314011111_CreateUser.Designer.cs
    │   ├── 20260314020615_AddRefreshTokens.cs              🆕 NEW
    │   ├── 20260314020615_AddRefreshTokens.Designer.cs     🆕 NEW
    │   └── AppDbContextModelSnapshot.cs                   ✏️ MOD
    ├── Repositories/
    │   ├── RefreshTokenRepository.cs                       🆕 NEW
    │   └── UserRepository.cs
    ├── Security/
    │   ├── JwtTokenService.cs                              🆕 NEW
    │   └── PasswordHasher.cs
    └── Infrastructure.csproj                              ✏️ MOD
```
