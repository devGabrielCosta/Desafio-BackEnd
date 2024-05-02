using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface ILocacaoService
    {
        Task InsertLocacaoAsync(Locacao locacao);
        decimal ConsultarDevolucao(Guid id, DateTime previsaoDevolucao, Guid entregadorId);
    }
}
