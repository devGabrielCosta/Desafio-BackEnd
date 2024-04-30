using Dominio.Entities;
using System.ComponentModel.DataAnnotations;

namespace Aplicacao.Requests
{
    public class UpdatePrevisaoDevolucao
    {
        [Required(ErrorMessage = "Data de devolução obrigatória")]
        public DateTime PrevisaoDevolucao { get; set; }
    }
}
