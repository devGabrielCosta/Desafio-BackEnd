using System.ComponentModel.DataAnnotations;

namespace Aplicacao.Requests
{
    public class CreatePedido
    {
        [Required(ErrorMessage = "Valor da corrida é obrigatório")]
        public decimal ValorDaCorrida { get; set; }
    }
}
