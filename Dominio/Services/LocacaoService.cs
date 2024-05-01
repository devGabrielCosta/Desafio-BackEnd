using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
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
                var message = "Nenhuma moto disponivel";
                _notificationContext.AddNotification(message);
                _logger.LogInformation(message);
                return;
            }

            var entregador = _entregadorService.GetLocacoes(locacao.EntregadorId);
            if(entregador == null)
            {
                _notificationContext.AddNotification("Entregador não existe");
                return;
            }

            if(entregador.Locacoes.Any(l => l.Ativo))
            {
                _notificationContext.AddNotification("Entregador já possui uma locação ativa");
                return;
            }

            if (!entregador.CnhTipo.ToLower().Contains("a"))
            {
                _notificationContext.AddNotification("Entregador não possui categoria A");
                return;
            }

            moto.Disponivel = false;
            _motoService.UpdateMoto(moto);

            locacao.Moto = moto;
            locacao.Entregador = entregador;

            await _repository.InsertAsync(locacao);
        }

        public decimal ConsultarDevolucao(Guid id, DateTime previsaoDevolucao)
        {
            var locacao = _repository.Get(id).FirstOrDefault();
            if (locacao == null)
            {
                _notificationContext.AddNotification("Locação não encontrada");
                return 0;
            }

            var plano = locacao.Plano;

            decimal preco = 0;

            var previsaoDate = previsaoDevolucao.Date;
            var inicioDate = locacao.Inicio.Date;
            var terminoDate = locacao.Termino.Date;

            if (previsaoDate == terminoDate)
            {
                preco = ((terminoDate - inicioDate).Days + 1) * PrecoPlano(plano);
            }
            else if (previsaoDate < terminoDate)
            {
                preco = ((previsaoDate - inicioDate).Days + 1) * PrecoPlano(plano);
                preco += (((terminoDate - previsaoDate).Days) * PrecoPlano(plano)) * MultaPlano(plano);
            }
            else if(previsaoDate > terminoDate)
            {
                preco = ((terminoDate - inicioDate).Days + 1) * PrecoPlano(plano);
                preco += ((previsaoDate - terminoDate).Days) * 50;
            }

            locacao.PrevisaoDevolucao = previsaoDevolucao;

            _repository.Update(locacao);

            return preco;

        }

        private int PrecoPlano(Plano plano)
        {
            if (plano is Plano.A)
                return 30;
            else if (plano is Plano.B)
                return 28;
            else if (plano is Plano.C)
                return 22;

            return 0;
        }

        private decimal MultaPlano(Plano plano)
        {
            if (plano is Plano.A)
                return 0.2m;
            else if (plano is Plano.B)
                return 0.4m;
            else if (plano is Plano.C)
                return 0.6m;

            return 0;
        }
    }

}
