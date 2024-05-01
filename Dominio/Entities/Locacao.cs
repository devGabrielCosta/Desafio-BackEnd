using System.Text.Json.Serialization;

namespace Dominio.Entities
{
    public class Locacao : BaseEntity
    {
        public Plano Plano { get; set; }
        public Guid EntregadorId { get; set; }
        public Guid MotoId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public DateTime PrevisaoDevolucao { get; set; }
        public bool Ativo { get; set; }

        [JsonIgnore]
        public virtual Moto Moto { get; set; } = null!;
        [JsonIgnore]
        public virtual Entregador Entregador { get; set; } = null!;

        public Locacao(Plano plano, Guid entregadorId) : base()
        {
            Plano = plano;
            EntregadorId = entregadorId;
            Inicio = DateTime.Now.AddDays(1);
            Ativo = true;

            if (Plano is Plano.A)
                Termino = Inicio.AddDays(6);
            else if (Plano is Plano.B)
                Termino = Inicio.AddDays(14);
            else if (Plano is Plano.C)
                Termino = Inicio.AddDays(29);
        }

    }

    public enum Plano
    {
        A,
        B,
        C
    }
}
