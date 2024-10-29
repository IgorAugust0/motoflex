using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motoflex.Domain.Entities;

namespace Motoflex.Infrastructure.Contexts
{
    public class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder
                .Property(m => m.Id)
                .IsRequired();

            builder
                .Property(m => m.Year)
                .HasColumnName("year")
                .IsRequired();

            builder
                .Property(m => m.Model)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasColumnName("model");
            //.HasMaxLength(50);

            builder
                .Property(m => m.LicensePlate)
                .IsRequired()
                .HasColumnType("char(7)")
                .HasColumnName("license_plate");

            builder.HasKey(m => m.Id);
            builder.HasIndex(m => m.LicensePlate).IsUnique();
        }
    }
}
