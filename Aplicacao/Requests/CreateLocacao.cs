using Dominio.Entities;
using System.ComponentModel.DataAnnotations;

namespace Aplicacao.Requests
{
    public class CreateLocacao
    {
        [Required(ErrorMessage = "O tipo de plano é obrigatório")]
        public Plano Plano { get; set; }

        [Required(ErrorMessage = "O entregador é obrigatório")]
        public Guid EntregadorId { get; set; }
    }
}
