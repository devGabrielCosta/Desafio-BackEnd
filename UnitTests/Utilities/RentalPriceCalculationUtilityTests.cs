using Domain.Entities;
using Domain.Utilities;
using UnitTests.Fixtures;

namespace UnitTests.Utilities
{
    public class RentalPriceCalculationUtilityTests
    {
        [Fact]
        public void Calcule_OnFinishDate_ShouldCalculateCorrectly()
        {
            // Arrange
            var rental = RentalFixture.Create(Plan.A);
            rental.ReturnAt = DateTime.Now.AddDays(7);

            // Act
            var price = RentalPriceCalculationUtility.Calculate(rental);

            // Assert
            Assert.Equal(210, price);
        }

        [Fact]
        public void Calcule_BeforeFinishDate_ShouldCalculateCorrectly()
        {
            // Arrange
            var rental = RentalFixture.Create(Plan.A);
            rental.ReturnAt = DateTime.Now.AddDays(4);

            // Act
            var price = RentalPriceCalculationUtility.Calculate(rental);

            // Assert
            Assert.Equal(138, price);
        }

        [Fact]
        public void Calcule_AfterFinishDate_ShouldCalculateCorrectly()
        {
            // Arrange
            var rental = RentalFixture.Create(Plan.A);
            rental.ReturnAt = DateTime.Now.AddDays(9);

            // Act
            var price = RentalPriceCalculationUtility.Calculate(rental);

            // Assert
            Assert.Equal(310, price);
        }
    }
}
