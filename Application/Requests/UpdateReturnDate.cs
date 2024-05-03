using System.ComponentModel.DataAnnotations;

namespace Application.Requests
{
    public class UpdateReturnDate
    {
        [Required(ErrorMessage = "Data de devolução obrigatória")]
        public DateTime ReturnDate { get; set; }
    }
}
