namespace Domain.Utilities
{
    public interface IPlan
    {
        decimal Price { get; }
        decimal PenaltyRate { get; }
    }

    public class PlanA : IPlan
    {
        public decimal Price => 30m;
        public decimal PenaltyRate => 0.2m;
    }

    public class PlanB : IPlan
    {
        public decimal Price => 28m;
        public decimal PenaltyRate => 0.4m;
    }

    public class PlanC : IPlan
    {
        public decimal Price => 22m;

        public decimal PenaltyRate => 0.6m;
    }
}
