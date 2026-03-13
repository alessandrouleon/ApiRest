using APIRest.Application.Interfaces;
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
using APIRest.API.Middlewares;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "APIRest - Users",
        Version = "v1",
        Description = "RESTful API for User management built with Clean Architecture and DDD."
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null)));

// Infrastructure
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Use Cases
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<FindAllUsersUseCase>();
builder.Services.AddScoped<FindUserByIdUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();

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
app.UseAuthorization();
app.MapControllers();

app.Run();
