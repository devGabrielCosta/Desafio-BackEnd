using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IEntregadorService
    {
        IEnumerable<Entregador> Get();
        Entregador Get(Guid Id);
        Task<Entregador> InsertEntregador(Entregador entregador);
        Task<Entregador> UpdateCnhImagemEntregador(Guid id, string imagem);
    }
}
