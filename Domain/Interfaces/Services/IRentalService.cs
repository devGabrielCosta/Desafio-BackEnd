using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IRentalService
    {
        Task InsertRentalAsync(Rental rental);
        decimal ReportReturn(Guid id, DateTime returnDate, Guid courierId);
    }
}
