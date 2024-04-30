using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;

namespace Dominio.Services
{
    public class EntregadorService : IEntregadorService
    {
        private IEntregadorRepository _repository { get; }

        public EntregadorService(IEntregadorRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<Entregador> Get()
        {
            return _repository.Get();
        }
        public Entregador Get(Guid id)
        {
            return _repository.Get(id);
        }

        public async Task<Entregador> InsertEntregador(Entregador entregador)
        {
            await _repository.InsertAsync(entregador);
            return entregador;
        }

        public async Task<Entregador> UpdateCnhImagemEntregador(Guid id, string imagem)
        {
            var entregador = _repository.Get(id);
            entregador.CnhImagem = imagem;
            await _repository.UpdateAsync(entregador);
            return entregador;
        }

    }
}
