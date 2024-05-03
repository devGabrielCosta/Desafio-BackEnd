using System.ComponentModel.DataAnnotations;

namespace Application.Requests
{
    public class CreateOrder
    {
        [Required(ErrorMessage = "Valor da corrida é obrigatório")]
        public decimal DeliveryFee { get; set; }
    }
}
