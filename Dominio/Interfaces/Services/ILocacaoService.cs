using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface ILocacaoService
    {
        Task<Locacao?> RealizarLocacao(Locacao locacao);
        Task<decimal> ConsultarDevolucao(Guid id, DateTime previsaoDevolucao);
    }
}
