using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts
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
                .Property<Guid>("CourierId")
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

            builder.HasKey("Id");
            builder.HasOne(e => e.Courier).WithMany(e => e.Rentals).HasForeignKey(e => e.CourierId);
            builder.HasOne(e => e.Motorcycle).WithMany(e => e.Rentals).HasForeignKey(e => e.MotorcycleId); ;
        }
    }
}
