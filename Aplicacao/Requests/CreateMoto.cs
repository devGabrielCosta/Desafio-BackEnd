using System.ComponentModel.DataAnnotations;

namespace Aplicacao.Requests
{
    public class CreateMoto
    {
        [Required(ErrorMessage = "O Ano é obrigatório")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "O Modelo é obrigatório")]
        public string? Modelo { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória")]
        [Length(7,7, ErrorMessage = "Placa deve ter 7 digitos")]
        public string? Placa { get; set; }
    }
}
