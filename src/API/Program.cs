using APIRest.Application.Interfaces;
using APIRest.Application.UseCases.Auth;
using APIRest.Application.UseCases.CreateUser;
using APIRest.Application.UseCases.DeleteUser;
using APIRest.Application.UseCases.FindAllUsers;
using APIRest.Application.UseCases.FindUserById;
using APIRest.Application.UseCases.UpdateUser;
using APIRest.Application.Validators;
using APIRest.Domain.Interfaces;
using APIRest.Infrastructure.Data;
using APIRest.Infrastructure.Repositories;
using APIRest.Infrastructure.Security;
using APIRest.API.Common;
using APIRest.API.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "APIRest - Users",
        Version = "v1",
        Description = "RESTful API for User management built with Clean Architecture and DDD."
    });
    options.UseInlineDefinitionsForEnums();
    options.UseAllOfToExtendReferenceSchemas();
    options.SchemaFilter<DefaultValueEnumSchemaFilter>();

    // JWT Bearer support in Swagger UI
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT access token. Example: eyJhbGci..."
    };
    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null)));

// JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSection["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };

        // Return ApiResponse-formatted JSON for 401/403 instead of empty responses
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.Fail("Authentication required. Please provide a valid token."));
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.Fail("Access denied. You do not have permission to perform this action."));
            }
        };
    });

builder.Services.AddAuthorization();

// Infrastructure
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>(); //New
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();  //New

// Use Cases - Users
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<FindAllUsersUseCase>();
builder.Services.AddScoped<FindUserByIdUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();

// Use Cases - Auth
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();
builder.Services.AddScoped<RevokeTokenUseCase>(); 

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "APIRest v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication(); //New
app.UseAuthorization();
app.MapControllers();

app.Run();
