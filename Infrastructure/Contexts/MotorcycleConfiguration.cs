using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts
{
    public class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property("Year")
                .IsRequired();

            builder
                .Property("Model")
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder
                .Property("LicensePlate")
                .IsRequired()
                .HasColumnType("char(7)");


            builder.HasKey("Id");
            builder.HasIndex("LicensePlate").IsUnique();
        }
    }
}
