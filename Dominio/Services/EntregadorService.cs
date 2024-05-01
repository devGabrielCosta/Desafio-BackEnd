using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Dominio.Services
{
    public class EntregadorService : IEntregadorService
    {
        private IEntregadorRepository _repository { get; }
        private INotificationContext _notificationContext { get; }
        private ILogger _logger { get; }

        public EntregadorService(
            IEntregadorRepository repository, 
            INotificationContext notificationContext,
            ILogger<EntregadorService> logger)
        {
            _repository = repository;
            _notificationContext = notificationContext;
            _logger = logger;
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

        public Entregador UpdateCnhImagemEntregador(Guid id, string imagem)
        {
            var entregador = _repository.Get(id).FirstOrDefault();

            _logger.LogInformation($"Imagem do entregadorId{entregador.Id} atualizada de {entregador.CnhImagem} para {imagem}");

            entregador.CnhImagem = imagem;
            _repository.Update(entregador);

            return entregador;
        }
    }
}
