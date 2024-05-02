namespace Aplicacao.Response
{
    public class PrevisaoLocacao
    {
        public decimal Preco{ get; set; }

        public PrevisaoLocacao(decimal preco)
        {
            Preco = preco;
        }
    }
}
