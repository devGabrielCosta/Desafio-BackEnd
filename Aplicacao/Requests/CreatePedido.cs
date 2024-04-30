using System.ComponentModel.DataAnnotations;

namespace Aplicacao.Requests
{
    public class CreatePedido
    {
        [Required(ErrorMessage = "Valor é obrigatório")]
        public decimal Valor { get; set; }
    }
}
