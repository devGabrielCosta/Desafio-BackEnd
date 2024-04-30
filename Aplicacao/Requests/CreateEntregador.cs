using System.ComponentModel.DataAnnotations;

namespace Aplicacao.Requests
{
    public class CreateEntregador
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cnh { get; set; }
        public string CnhTipo { get; set; }
    }
}
