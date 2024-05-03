using System.ComponentModel.DataAnnotations;

namespace Application.Requests
{
    public class UpdateMotorcycle
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [Length(7, 7, ErrorMessage = "Placa deve ter 7 digitos")]
        public string LicensePlate { get; set; }
    }
}
