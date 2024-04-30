using System.ComponentModel.DataAnnotations;

namespace Aplicacao.Requests
{
    public class CreateEntregador
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [RegularExpression(@"[0-9]{14}", ErrorMessage = "CNPJ deve ter 14 digitos")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "A CNH é obrigatória")]
        [RegularExpression(@"[0-9]{12}", ErrorMessage = "CNH deve ter 12 digitos")]
        public string Cnh { get; set; }

        [Required(ErrorMessage = "O tipo de CNH é obrigatório")]
        [RegularExpression(@"[a-eA-E]{1,5}", ErrorMessage = "Tipo de CNH pode ter no máximo 5 caracteres de 'A' a 'E'")]
        public string CnhTipo { get; set; }

    }
}
