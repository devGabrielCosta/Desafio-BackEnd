using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests
{
    public class CreateRental
    {
        [Required(ErrorMessage = "O tipo de plano é obrigatório")]
        public Plan Plan { get; set; }
    }
}
