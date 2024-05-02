using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Context
{
    public class EntregadorConfiguration : IEntityTypeConfiguration<Entregador>
    {
        public void Configure(EntityTypeBuilder<Entregador> builder)
        {
            builder
                .Property<Guid>("Id")
                .IsRequired();

            builder
                .Property("Nome")
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property("Cnpj")
                .HasColumnType("char(14)")
                .IsRequired();

            builder
                .Property<DateTime>("DataNascimento")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property("Cnh")
                .HasColumnType("char(12)")
                .IsRequired();

            builder
                .Property("CnhTipo")
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired();

            builder
                .Property("CnhImagem")
                .HasColumnType("varchar");


            builder.HasKey("Id");
            builder.HasIndex("Cnpj").IsUnique();
            builder.HasIndex("Cnh").IsUnique();

            builder
                .HasMany(e => e.Notificacoes)
                .WithMany(e => e.Notificados)
                .UsingEntity("Notificacoes",
                    j =>{   
                            j.Property("NotificacoesId").HasColumnName("PedidoId");
                            j.Property("NotificadosId").HasColumnName("EntregadorId");
                    }
                );
        }
    }
}
