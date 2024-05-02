using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using Dominio.Interfaces.Storage;
using Microsoft.Extensions.Logging;

namespace Dominio.Services
{
    public class EntregadorService : IEntregadorService
    {
        private IEntregadorRepository _repository { get; }
        private INotificationContext _notificationContext { get; }
        private ILogger _logger { get; }
        private IStorage _storage { get; }

        public EntregadorService(
            IEntregadorRepository repository, 
            INotificationContext notificationContext,
            ILogger<EntregadorService> logger,
            IStorage storage)
        {
            _repository = repository;
            _notificationContext = notificationContext;
            _logger = logger;
            _storage = storage;
        }
        public IEnumerable<Entregador> Get()
        {
            return _repository.Get();
        }
        public Entregador Get(Guid id)
        {
            return _repository.Get(id).FirstOrDefault();
        }
        public Entregador GetLocacoes(Guid id)
        {
            return _repository.GetLocacoes().FirstOrDefault(e => e.Id == id);
        }

        public async Task InsertEntregadorAsync(Entregador entregador)
        {
            var cnhUsado = _repository.Get().Where(x => x.Cnh == entregador.Cnh).Any();
            var cnpjUsado = _repository.Get().Where(x => x.Cnpj == entregador.Cnpj).Any();

            if (cnhUsado)
                _notificationContext.AddNotification("CNH já utilizada");

            if (cnpjUsado)
                _notificationContext.AddNotification("CNPJ já utilizadO");

            if (_notificationContext.HasNotifications)
                return;

            await _repository.InsertAsync(entregador);
        }

        public async Task<Entregador> UpdateCnhImagemEntregador(Guid id, Utilities.File imagem)
        {
            var entregador = _repository.Get(id).FirstOrDefault();
            if (entregador == null)
            {
                _notificationContext.AddNotification("Entregador não encontrado");
                return null;
            }
            if(!imagem.Type.Contains("png") && !imagem.Type.Contains("bmp"))
            {
                _notificationContext.AddNotification("A imagem deve ser do tipo png ou bmp");
                return null;
            }

            var imagemUrl = await _storage.UploadFile(imagem.Stream, $"CNH-{entregador.Id}.{imagem.Type}");

            _logger.LogInformation($"Imagem do entregadorId{entregador.Id} atualizada de {entregador.CnhImagem} para {imagemUrl}");

            entregador.CnhImagem = imagemUrl;
            _repository.Update(entregador);

            return entregador;
        }
    }
}
