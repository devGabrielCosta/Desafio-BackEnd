using System.Text.Json.Serialization;

namespace Dominio.Entities
{
    public class Pedido : BaseEntity
    {
        public DateTime Criado { get; set; }
        public decimal ValorDaCorrida {  get; set; }
        public Situacao Situacao { get; set; }

        public Guid? EntregadorId { get; set; }

        [JsonIgnore]
        public virtual Entregador Entregador { get; set; } = null!; 

        [JsonIgnore]
        public virtual ICollection<Entregador> Notificados { get; set; } = new List<Entregador>();

        public Pedido(decimal valorDaCorrida)
        {
            Criado = DateTime.Now;
            Situacao = Situacao.Disponivel;
            ValorDaCorrida = valorDaCorrida;
        }

    }

    public enum Situacao
    {
        Disponivel,
        Aceito,
        Entregue,
    }
}
