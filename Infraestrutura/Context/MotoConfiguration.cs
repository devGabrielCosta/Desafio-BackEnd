using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Context
{
    public class MotoConfiguration : IEntityTypeConfiguration<Moto>
    {
        public void Configure(EntityTypeBuilder<Moto> builder)
        {
            builder
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property("Ano")
                .IsRequired()
                .HasMaxLength(4);

            builder
                .Property("Modelo")
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder
                .Property("Placa")
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength (7);


            builder.HasKey("Id");
            builder.HasIndex("Placa");
        }
    }
}
