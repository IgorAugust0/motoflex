using Motoflex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Motoflex.Infrastructure.Contexts
{
    public class RenterConfiguration : IEntityTypeConfiguration<Renter>
    {
        public void Configure(EntityTypeBuilder<Renter> builder)
        {
            builder
                .Property(m => m.Id)
                .IsRequired();

            builder
                .Property(m => m.Name)
                .HasColumnType("varchar")
                .HasColumnName("name")
                .IsRequired();

            builder
                .Property(m => m.Cnpj)
                .HasColumnType("char(14)")
                .HasColumnName("cnpj")
                .IsRequired();

            builder
                .Property(m => m.Birthdate)
                .HasColumnType("Date")
                .HasColumnName("birthdate")
                .IsRequired();

            builder
                .Property(m => m.Cnh)
                .HasColumnType("char(12)")
                .HasColumnName("cnh")
                .IsRequired();

            builder
                .Property(m => m.CnhType)
                .HasConversion<string>()
                .HasColumnType("varchar")
                .HasColumnName("cnh_type")
                .IsRequired();

            builder
                .Property(m => m.CnhImage)
                .HasColumnType("varchar")
                .HasColumnName("cnh_image");

            builder.HasKey("Id");
            builder.HasIndex(m => m.Cnpj).IsUnique();
            builder.HasIndex(m => m.Cnh).IsUnique();

            // Configures many-to-many relationship between Renter and Notification entities
            builder
                .HasMany(m => m.Notifications)
                .WithMany(m => m.NotifiedRenters)
                .UsingEntity("Notifications", j =>
                {
                    // Sets column names for join table properties
                    j.Property("NotificationsId").HasColumnName("OrderId");
                    j.Property("NotifiedRentersId").HasColumnName("RenterId");
                });
        }
    }
}
