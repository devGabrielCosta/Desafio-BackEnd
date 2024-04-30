using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using System.Numerics;

namespace Dominio.Services
{
    public class LocacaoService : ILocacaoService
    {
        private ILocacaoRepository _repository { get; }
        private IEntregadorService _entregadorService { get; }
        private IMotoService _motoService { get; }

        public LocacaoService(ILocacaoRepository repository, IEntregadorService entregadorService, IMotoService motoService)
        {
            _repository = repository;
            _entregadorService = entregadorService;
            _motoService = motoService;
        }

        public async Task<Locacao?> RealizarLocacao(Locacao locacao)
        {
            var moto = _motoService.GetMotosDisponiveis().Where(m => m.Disponivel).FirstOrDefault();
            if (moto == null)
                return null;

            var entregador = _entregadorService.Get(locacao.EntregadorId);
            if (!entregador.CnhTipo.ToLower().Contains("a"))
                return null;

            moto.Disponivel = false;
            _motoService.UpdateMoto(moto);

            locacao.Moto = moto;
            locacao.Entregador = entregador;

            await _repository.InsertAsync(locacao);

            return locacao;
        }

        public async Task<decimal> ConsultarDevolucao(Guid id, DateTime previsaoDevolucao)
        {
            var locacao = _repository.Get(id);
            var plano = locacao.Plano;

            decimal preco = 0;

            if (previsaoDevolucao == locacao.Termino)
            {
                preco = ((locacao.Termino - locacao.Inicio).Days + 1) * PrecoPlano(plano);
            }
            else if (previsaoDevolucao < locacao.Termino)
            {
                preco = ((previsaoDevolucao - locacao.Inicio).Days + 1) * PrecoPlano(plano);
                preco += (((locacao.Termino - previsaoDevolucao).Days + 1) * PrecoPlano(plano)) * MultaPlano(plano);
            }
            else if(previsaoDevolucao > locacao.Termino)
            {
                preco = ((locacao.Termino - locacao.Inicio).Days + 1) * PrecoPlano(plano);
                preco += ((previsaoDevolucao - locacao.Termino).Days) * 50;
            }

            locacao.PrevisaoDevolucao = previsaoDevolucao;
            await _repository.UpdateAsync(locacao);

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
                return 1.2m;
            else if (plano is Plano.B)
                return 1.4m;
            else if (plano is Plano.C)
                return 1.6m;

            return 0;
        }
    }

}
