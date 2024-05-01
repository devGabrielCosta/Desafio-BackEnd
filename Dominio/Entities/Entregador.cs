using System.Text.Json.Serialization;

namespace Dominio.Entities
{
    public class Entregador : BaseEntity
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cnh { get; set; }
        public string CnhTipo { get; set; }
        public string? CnhImagem { get; set; }

        [JsonIgnore]
        public virtual ICollection<Locacao> Locacoes { get; set; } = new List<Locacao>();
        [JsonIgnore]
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        [JsonIgnore]
        public virtual ICollection<Pedido> Notificacoes { get; set; } = new List<Pedido>();

        public Entregador(string nome, string cnpj, DateTime dataNascimento, string cnh, string cnhTipo) : base()
        {
            Nome = nome;
            Cnpj = cnpj;
            DataNascimento = dataNascimento;
            Cnh = cnh;
            CnhTipo = cnhTipo;
        }
    }
}
