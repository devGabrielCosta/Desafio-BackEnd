namespace Dominio.Utilities
{
    public interface IPlano
    {
        decimal Preco { get; }
        decimal Multa { get; }
    }

    public class PlanoA : IPlano
    {
        public decimal Preco => 30m;
        public decimal Multa => 0.2m;
    }

    public class PlanoB : IPlano
    {
        public decimal Preco => 28m;
        public decimal Multa => 0.4m;
    }

    public class PlanoC : IPlano
    {
        public decimal Preco => 22m;

        public decimal Multa => 0.6m;
    }
}
