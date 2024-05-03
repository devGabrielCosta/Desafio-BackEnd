using System.ComponentModel.DataAnnotations;

namespace Application.Requests
{
    public class CreateMotorcycle
    {
        [Required(ErrorMessage = "O Ano é obrigatório")]
        public int Year { get; set; }

        [Required(ErrorMessage = "O Modelo é obrigatório")]
        public string? Model { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória")]
        [Length(7,7, ErrorMessage = "Placa deve ter 7 digitos")]
        public string? LicensePlate { get; set; }
    }
}
