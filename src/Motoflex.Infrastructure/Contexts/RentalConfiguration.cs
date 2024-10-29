using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motoflex.Domain.Entities;

namespace Motoflex.Infrastructure.Contexts
{
    public class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property<Plan>("Plan")
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property<Guid>("RenterId")
                .IsRequired();

            builder
                .Property<Guid>("MotorcycleId")
                .IsRequired();

            builder
                .Property<DateTime>("BeginAt")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property<DateTime>("FinishAt")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property<DateTime>("ReturnAt")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property<bool>("Active")
                .IsRequired();

            builder.HasKey("Id"); // Sets primary key

            // Configures one-to-many relationship between Renter and Rentals
            builder
                .HasOne(o => o.Renter)
                .WithMany(o => o.Rentals)
                .HasForeignKey(o => o.RenterId);

            // Configures one-to-many relationship between Motorcycle and Rentals
            builder
                .HasOne(o => o.Motorcycle)
                .WithMany(o => o.Rentals)
                .HasForeignKey(o => o.MotorcycleId);
        }
    }
}
