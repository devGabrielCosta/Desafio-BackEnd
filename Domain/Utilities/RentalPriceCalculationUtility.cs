using Domain.Entities;

namespace Domain.Utilities
{
    public static class RentalPriceCalculationUtility
    {
        private const decimal DAILY_PRICE_AFTER_FINISH = 50;

        public static decimal Calculate(Rental rental)
        {
            IPlan plan = GetPlan(rental.Plan);

            var returnDate = rental.ReturnAt.Date;
            var beginDate = rental.BeginAt.Date;
            var finishDate = rental.FinishAt.Date;

            if (returnDate < finishDate)
                return CalculatePriceBeforeFinishDate(beginDate, finishDate, returnDate, plan);
            else if (returnDate > finishDate)
                return CalculatePriceAfterFinishDate(beginDate, finishDate, returnDate, plan);
            
            return CalculatePriceOnFinishDate(beginDate, finishDate, plan);
        }

        private static decimal CalculatePriceOnFinishDate(DateTime beginDate, DateTime finishDate, IPlan plan)
        {
            return ((finishDate - beginDate).Days + 1) * plan.Price;
        }

        private static decimal CalculatePriceBeforeFinishDate(DateTime beginDate, DateTime finishDate, DateTime returnDate, IPlan plan)
        {
            var preco = ((returnDate - beginDate).Days + 1) * plan.Price;
            preco += ((finishDate - returnDate).Days) * plan.Price * plan.PenaltyRate;
            return preco;
        }

        private static decimal CalculatePriceAfterFinishDate(DateTime beginDate, DateTime finishDate, DateTime returnDate, IPlan plan)
        {
            var preco = ((finishDate - beginDate).Days + 1) * plan.Price;
            preco += ((returnDate - finishDate).Days) * DAILY_PRICE_AFTER_FINISH;
            return preco;
        }

        private static IPlan GetPlan(Plan plan)
        {
            switch (plan)
            {
                case Plan.A:
                    return new PlanA();
                case Plan.B:
                    return new PlanB();
                case Plan.C:
                    return new PlanC();
                default:
                    throw new ArgumentException("Unknow plan", nameof(plan));
            }
        }
    }

}
