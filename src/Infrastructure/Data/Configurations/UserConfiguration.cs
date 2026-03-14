using APIRest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIRest.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasDatabaseName("IX_Users_Username");

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.OwnsOne(u => u.Email, emailNav =>
        {
            emailNav.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(255);

            emailNav.HasIndex(e => e.Value)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");
        });

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired();

        builder.Property(u => u.DeletedAt)
            .IsRequired(false);
    }
}
