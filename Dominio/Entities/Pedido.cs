namespace Dominio.Entities
{
    public class Pedido : BaseEntity
    {
        public DateTime Criado { get; set; }
        public decimal Valor {  get; set; }
        public Situacao Situacao { get; set; }

        public Guid? EntregadorId { get; set; }
        public virtual Entregador Entregador { get; set; } = null!;
        public virtual ICollection<Entregador> Notificados { get; set; } = new List<Entregador>();

        public Pedido(decimal valor)
        {
            Criado = DateTime.Now;
            Situacao = Situacao.Disponivel;
            Valor = valor;
        }

    }

    public enum Situacao
    {
        Disponivel,
        Aceito,
        Entregue,
    }
}
