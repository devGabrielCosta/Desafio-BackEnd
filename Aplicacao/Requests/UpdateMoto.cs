using System.ComponentModel.DataAnnotations;

namespace Aplicacao.Requests
{
    public class UpdateMoto
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [Length(7, 7, ErrorMessage = "Placa deve ter 7 digitos")]
        public string Placa { get; set; }
    }
}
