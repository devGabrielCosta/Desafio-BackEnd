using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using Dominio.Utilities;
using Microsoft.Extensions.Logging;

namespace Dominio.Services
{
    public class LocacaoService : ILocacaoService
    {
        private ILocacaoRepository _repository { get; }
        private IEntregadorService _entregadorService { get; }
        private IMotoService _motoService { get; }
        private INotificationContext _notificationContext { get; }
        private ILogger _logger { get; }

        public LocacaoService(
            ILocacaoRepository repository, 
            IEntregadorService entregadorService, 
            IMotoService motoService, 
            INotificationContext notificationContext,
            ILogger<LocacaoService> logger)
        {
            _repository = repository;
            _entregadorService = entregadorService;
            _motoService = motoService;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public async Task InsertLocacaoAsync(Locacao locacao)
        {
            var moto = _motoService.GetMotosDisponiveis().FirstOrDefault();
            if (moto == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.NENHUMA_MOTO_DISPONIVEL);
                _logger.LogInformation(ErrorNotifications.NENHUMA_MOTO_DISPONIVEL);
                return;
            }

            var entregador = _entregadorService.GetLocacoes(locacao.EntregadorId);
            if(entregador == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.ENTREGADOR_NAO_ENCONTRADO);
                return;
            }

            if(entregador.Locacoes.Any(l => l.Ativo))
            {
                _notificationContext.AddNotification(ErrorNotifications.ENTREGADOR_LOCACAO_ATIVA);
                return;
            }

            if (entregador.CnhTipo == CnhTipo.B )
            {
                _notificationContext.AddNotification(ErrorNotifications.ENTREGADOR_SEM_CATEGORIA_A);
                return;
            }

            moto.Disponivel = false;
            _motoService.UpdateMoto(moto);

            locacao.Moto = moto;
            locacao.Entregador = entregador;

            await _repository.InsertAsync(locacao);
        }

        public decimal ConsultarDevolucao(Guid id, DateTime previsaoDevolucao, Guid entregadorId)
        {
            var locacao = _repository.Get(id).FirstOrDefault();
            if (locacao == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.LOCACAO_NAO_ENCONTRADA);
                return 0;
            }
            if(entregadorId != locacao.EntregadorId)
            {
                _notificationContext.AddNotification(ErrorNotifications.LOCACAO_ENTREGADOR_SEM_PERMISSAO);
                return 0;
            }

            locacao.PrevisaoDevolucao = previsaoDevolucao;

            _repository.Update(locacao);

            return CalculoValorLocacaoUtility.CalcularValor(locacao);
        }
    }

}
