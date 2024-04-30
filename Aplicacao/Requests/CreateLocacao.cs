using Dominio.Entities;

namespace Aplicacao.Requests
{
    public class CreateLocacao
    {
        public Plano Plano { get; set; }
        public Guid EntregadorId { get; set; }
    }
}
