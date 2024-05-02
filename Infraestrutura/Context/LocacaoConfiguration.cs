using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Context
{
    public class LocacaoConfiguration : IEntityTypeConfiguration<Locacao>
    {
        public void Configure(EntityTypeBuilder<Locacao> builder)
        {
            builder
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property<Plano>("Plano")
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property<Guid>("EntregadorId")
                .IsRequired();

            builder
                .Property<Guid>("MotoId")
                .IsRequired();

            builder
                .Property<DateTime>("Inicio")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property<DateTime>("Termino")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property<DateTime>("Devolucao")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property<bool>("Ativo")
                .IsRequired();

            builder.HasKey("Id");
            builder.HasOne("Entregador").WithMany("Locacoes").HasForeignKey("EntregadorId");
            builder.HasOne("Moto").WithMany("Locacoes").HasForeignKey("MotoId"); ;
        }
    }
}
