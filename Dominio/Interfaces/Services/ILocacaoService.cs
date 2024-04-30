using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface ILocacaoService
    {
        Task InsertLocacaoAsync(Locacao locacao);
        object ConsultarDevolucao(Guid id, DateTime previsaoDevolucao);
    }
}
