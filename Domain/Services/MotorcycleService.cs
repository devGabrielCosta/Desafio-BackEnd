using Domain.Entities;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Utilities;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        public IMotoRepository _repository { get; }
        private INotificationContext _notificationContext { get; }
        private ILogger _logger { get; }

        public MotorcycleService(
            IMotoRepository repository, 
            INotificationContext notificationContext,
            ILogger<MotorcycleService> logger)
        {
            _repository = repository;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public IEnumerable<Motorcycle> GetByLicensePlate(string licensePlate)
        {
            return _repository.GetByLicensePlate(licensePlate).ToList();
        }
        public IEnumerable<Motorcycle> GetAvailable()
        {
            return _repository.Get().Where(m => m.Available);
        }

        public async Task InsertMotorcycleAsync(Motorcycle motorcycle)
        {   
            var motosComMesmaPlaca = this.GetByLicensePlate(motorcycle.LicensePlate).Any();
            if(motosComMesmaPlaca)
            {
                _notificationContext.AddNotification(ErrorNotifications.LICENSE_PLATE_USED);
                return;
            }

            await _repository.InsertAsync(motorcycle);
        }

        public Motorcycle UpdateMotorcycleLicensePlate(Guid id, string licensePlate)
        {
            var motorcycle = _repository.Get(id).FirstOrDefault();
            if (motorcycle == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.MOTORCYCLE_NOT_FOUND);
                return null;
            }

            var motosWithSameLicensePlate = this.GetByLicensePlate(licensePlate).Any();
            if (motosWithSameLicensePlate)
            {
                _notificationContext.AddNotification(ErrorNotifications.LICENSE_PLATE_USED);
                return null;
            }

            _logger.LogInformation($"MotorcycleId:{motorcycle.Id}. LicensePlate from {motorcycle.LicensePlate} to {licensePlate}");

            motorcycle.LicensePlate = licensePlate;
            this.UpdateMotorcycle(motorcycle);

            return motorcycle;
        }

        public void UpdateMotorcycle(Motorcycle motorcycle)
        {
            _repository.Update(motorcycle);
        }

        public void DeleteMotorcycle(Guid id)
        {   
            var motorcycle = _repository.GetRentals().Where(m => m.Id == id).FirstOrDefault();

            if (motorcycle == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.MOTORCYCLE_NOT_FOUND);
                return;
            }
            if(motorcycle.Rentals.Any())
            {
                _notificationContext.AddNotification(ErrorNotifications.MOTORCYCLE_WITH_RENTALS);
                return;
            }

            _repository.Delete(id);

            _logger.LogInformation($"MotorcycleId:{motorcycle.Id}. Deleted.");
        }
    }
}
