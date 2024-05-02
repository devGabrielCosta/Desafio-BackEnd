using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IEntregadorService
    {
        IEnumerable<Entregador> Get();
        Entregador Get(Guid Id);
        Entregador GetLocacoes(Guid Id);
        Task InsertEntregadorAsync(Entregador entregador);
        Task<Entregador> UpdateCnhImagemEntregador(Guid id, Utilities.File uploadImage);
    }
}
