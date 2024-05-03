using Domain.Entities;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Utilities;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class RentalService : IRentalService
    {
        private IRentalRepository _repository { get; }
        private ICourierService _courierService { get; }
        private IMotorcycleService _motorcycleService { get; }
        private INotificationContext _notificationContext { get; }
        private ILogger _logger { get; }

        public RentalService(
            IRentalRepository repository, 
            ICourierService courierService, 
            IMotorcycleService motorcycleService, 
            INotificationContext notificationContext,
            ILogger<RentalService> logger)
        {
            _repository = repository;
            _courierService = courierService;
            _motorcycleService = motorcycleService;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public async Task InsertRentalAsync(Rental rental)
        {
            var motorcycle = _motorcycleService.GetAvailable().FirstOrDefault();
            if (motorcycle == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.NO_MOTORCYCLES_AVAILABLE);
                _logger.LogInformation(ErrorNotifications.NO_MOTORCYCLES_AVAILABLE);
                return;
            }

            var courier = _courierService.GetRentals(rental.CourierId);
            if(courier == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.COURIER_NOT_FOUND);
                return;
            }

            if(courier.Rentals.Any(l => l.Active))
            {
                _notificationContext.AddNotification(ErrorNotifications.COURIER_RENTAL_ACTIVE);
                return;
            }

            if (courier.CnhType == CnhType.B )
            {
                _notificationContext.AddNotification(ErrorNotifications.COURIER_WITHOUT_CNHTYPE_A);
                return;
            }

            motorcycle.Available = false;
            _motorcycleService.UpdateMotorcycle(motorcycle);

            rental.Motorcycle = motorcycle;
            rental.Courier = courier;

            await _repository.InsertAsync(rental);
        }

        public decimal ReportReturn(Guid id, DateTime returnDate, Guid courierId)
        {
            var rental = _repository.Get(id).FirstOrDefault();
            if (rental == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.RENTAL_NOT_FOUND);
                return 0;
            }
            if (courierId != rental.CourierId)
            {
                _notificationContext.AddNotification(ErrorNotifications.RENTAL_COURIER_NOT_AUTHORIZED);
                return 0;
            }
            if (!rental.Active)
            {
                _notificationContext.AddNotification(ErrorNotifications.RENTAL_INACTIVE);
                return 0;
            }

            rental.ReturnAt = returnDate;
            rental.Active = false;
            rental.Motorcycle.Available = true;

            _repository.Update(rental);

            return RentalPriceCalculationUtility.Calculate(rental);
        }
    }

}
