using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;

namespace Dominio.Services
{
    public class MotoService : IMotoService
    {
        public IMotoRepository _repository { get; }

        public MotoService(IMotoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Moto> GetByPlaca(string placa)
        {
            return _repository.GetByPlaca(placa).ToList();
        }
        public IEnumerable<Moto> GetMotosDisponiveis()
        {
            return _repository.Get();
        }

        public async Task<Moto> InsertMoto(Moto moto)
        {
            await _repository.InsertAsync(moto);
            return moto;
        }

        public async Task<Moto> UpdatePlacaMoto(Guid id, string placa)
        {
            var moto = _repository.Get(id);
            moto.Placa = placa;
            await _repository.UpdateAsync(moto);
            return moto;
        }

        public async void UpdateMoto(Moto moto)
        {
            await _repository.UpdateAsync(moto);
        }

        public async Task DeleteMoto(Guid id)
        {
           await _repository.DeleteAsync(id);
        }
    }
}
