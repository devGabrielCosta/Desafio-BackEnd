using Domain.Entities;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.Storage;
using Domain.Utilities;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class CourierService : ICourierService
    {
        private ICourierRepository _repository { get; }
        private INotificationContext _notificationContext { get; }
        private ILogger _logger { get; }
        private IStorage _storage { get; }

        public CourierService(
            ICourierRepository repository, 
            INotificationContext notificationContext,
            ILogger<CourierService> logger,
            IStorage storage)
        {
            _repository = repository;
            _notificationContext = notificationContext;
            _logger = logger;
            _storage = storage;
        }
        public IEnumerable<Courier> Get()
        {
            return _repository.Get();
        }
        public Courier Get(Guid id)
        {
            return _repository.Get(id).FirstOrDefault();
        }
        public Courier GetRentals(Guid id)
        {
            return _repository.GetRentals().FirstOrDefault(e => e.Id == id);
        }

        public async Task InsertCourierAsync(Courier courier)
        {
            var cnhUsado = _repository.Get().Where(x => x.Cnh == courier.Cnh).Any();
            var cnpjUsado = _repository.Get().Where(x => x.Cnpj == courier.Cnpj).Any();

            if (cnhUsado)
                _notificationContext.AddNotification(ErrorNotifications.CNH_USED);

            if (cnpjUsado)
                _notificationContext.AddNotification(ErrorNotifications.CNPJ_USED);

            if (_notificationContext.HasNotifications)
                return;

            await _repository.InsertAsync(courier);
        }

        public async Task<Courier> UpdateCourierCnhImage(Guid id, Utilities.File image)
        {
            var courier = _repository.Get(id).FirstOrDefault();
            if (courier == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.COURIER_NOT_FOUND);
                return null;
            }
            if(!image.Type.Contains("png") && !image.Type.Contains("bmp"))
            {
                _notificationContext.AddNotification(ErrorNotifications.IMAGE_INVALID_TYPE);
                return null;
            }

            var imageUrl = await _storage.UploadFile(image.Stream, $"CNH-{courier.Id}.{image.Type}");

            _logger.LogInformation($"Image of courierId:{courier.Id} updates from {courier.CnhImage} to {imageUrl}");

            courier.CnhImage = imageUrl;
            _repository.Update(courier);

            return courier;
        }
    }
}
