using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIRest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReorderUserColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TABLE [Users_Reordered] (
                    [Id]           uniqueidentifier  NOT NULL,
                    [Name]         nvarchar(100)     NOT NULL,
                    [Username]     nvarchar(50)      NOT NULL,
                    [Email]        nvarchar(255)     NOT NULL,
                    [IsActive]     bit               NOT NULL DEFAULT CAST(1 AS bit),
                    [PasswordHash] nvarchar(500)     NOT NULL,
                    [CreatedAt]    datetime2         NOT NULL,
                    [UpdatedAt]    datetime2         NOT NULL,
                    [DeletedAt]    datetime2         NULL,
                    CONSTRAINT [PK_Users_Reordered] PRIMARY KEY ([Id])
                );

                INSERT INTO [Users_Reordered]
                    ([Id],[Name],[Username],[Email],[IsActive],[PasswordHash],[CreatedAt],[UpdatedAt],[DeletedAt])
                SELECT
                    [Id],[Name],[Username],[Email],[IsActive],[PasswordHash],[CreatedAt],[UpdatedAt],[DeletedAt]
                FROM [Users];

                DROP TABLE [Users];

                EXEC sp_rename N'Users_Reordered', N'Users';
                EXEC sp_rename N'PK_Users_Reordered', N'PK_Users', N'OBJECT';

                CREATE UNIQUE INDEX [IX_Users_Email]    ON [Users] ([Email]);
                CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TABLE [Users_Reordered] (
                    [Id]           uniqueidentifier  NOT NULL,
                    [Name]         nvarchar(100)     NOT NULL,
                    [Email]        nvarchar(255)     NOT NULL,
                    [PasswordHash] nvarchar(500)     NOT NULL,
                    [CreatedAt]    datetime2         NOT NULL,
                    [UpdatedAt]    datetime2         NOT NULL,
                    [DeletedAt]    datetime2         NULL,
                    [IsActive]     bit               NOT NULL DEFAULT CAST(1 AS bit),
                    [Username]     nvarchar(50)      NOT NULL,
                    CONSTRAINT [PK_Users_Reordered] PRIMARY KEY ([Id])
                );

                INSERT INTO [Users_Reordered]
                    ([Id],[Name],[Email],[PasswordHash],[CreatedAt],[UpdatedAt],[DeletedAt],[IsActive],[Username])
                SELECT
                    [Id],[Name],[Email],[PasswordHash],[CreatedAt],[UpdatedAt],[DeletedAt],[IsActive],[Username]
                FROM [Users];

                DROP TABLE [Users];

                EXEC sp_rename N'Users_Reordered', N'Users';
                EXEC sp_rename N'PK_Users_Reordered', N'PK_Users', N'OBJECT';

                CREATE UNIQUE INDEX [IX_Users_Email]    ON [Users] ([Email]);
                CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);
            ");
        }
    }
}
