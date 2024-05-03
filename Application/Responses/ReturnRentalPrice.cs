namespace Application.Responses
{
    public class ReturnRentalPrice
    {
        public decimal Price{ get; set; }

        public ReturnRentalPrice(decimal preco)
        {
            Price = preco;
        }
    }
}
