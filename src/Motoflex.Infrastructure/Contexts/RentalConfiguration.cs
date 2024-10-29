using Motoflex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Motoflex.Infrastructure.Contexts
{
    public class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder
                .Property(m => m.Id)
                .IsRequired();

            builder
                .Property(m => m.Plan)
                .HasConversion<string>()
                .HasColumnType("varchar")
                .HasColumnName("plan")
                .IsRequired();

            builder
                .Property(m => m.RenterId)
                .HasColumnName("renter_id")
                .IsRequired();

            builder
                .Property(m => m.MotorcycleId)
                .HasColumnName("motorcycle_id")
                .IsRequired();

            builder
                .Property(m => m.BeginAt)
                .HasColumnType("Date")
                .HasColumnName("begin_at")
                .IsRequired();

            builder
                .Property(m => m.FinishAt)
                .HasColumnType("Date")
                .HasColumnName("finish_at")
                .IsRequired();

            builder
                .Property(m => m.ReturnAt)
                .HasColumnType("Date")
                .HasColumnName("return_at")
                .IsRequired();

            builder
                .Property(m => m.Active)
                .HasColumnName("active")
                .IsRequired();

            builder.HasKey(m => m.Id); // Sets primary key

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
