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
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property("Name")
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property("Cnpj")
                .HasColumnType("char(14)")
                .IsRequired();

            builder
                .Property<DateTime>("BirthDate")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property("Cnh")
                .HasColumnType("char(12)")
                .IsRequired();

            builder
                .Property("CnhType")
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property("CnhImage")
                .HasColumnType("varchar");

            builder.HasKey("Id");
            builder.HasIndex("Cnpj").IsUnique();
            builder.HasIndex("Cnh").IsUnique();

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
