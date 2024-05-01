﻿using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace Dominio.Services
{
    public class MotoService : IMotoService
    {
        public IMotoRepository _repository { get; }
        private INotificationContext _notificationContext { get; }
        private ILogger _logger { get; }

        public MotoService(
            IMotoRepository repository, 
            INotificationContext notificationContext,
            ILogger<MotoService> logger)
        {
            _repository = repository;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public IEnumerable<Moto> GetByPlaca(string placa)
        {
            return _repository.GetByPlaca(placa).ToList();
        }
        public IEnumerable<Moto> GetMotosDisponiveis()
        {
            return _repository.Get().Where(m => m.Disponivel);
        }

        public async Task InsertMotoAsync(Moto moto)
        {   
            var motosComMesmaPlaca = this.GetByPlaca(moto.Placa).Any();
            if(motosComMesmaPlaca)
            {
                _notificationContext.AddNotification("Placa já utilizada");
                return;
            }

            await _repository.InsertAsync(moto);
        }

        public Moto UpdatePlacaMoto(Guid id, string placa)
        {
            var moto = _repository.Get(id).FirstOrDefault();
            if (moto == null)
            {
                _notificationContext.AddNotification("Moto não encontrada");
                return null;
            }

            var motosComMesmaPlaca = this.GetByPlaca(placa).Any();
            if (motosComMesmaPlaca)
            {
                _notificationContext.AddNotification("Placa já utilizada");
                return null;
            }

            _logger.LogInformation($"MotoId:{moto.Id}. Placa atualizada de {moto.Placa} para {placa}");

            moto.Placa = placa;
            this.UpdateMoto(moto);

            return moto;
        }

        public void UpdateMoto(Moto moto)
        {
            _repository.Update(moto);
        }

        public void DeleteMoto(Guid id)
        {   
            var moto = _repository.GetLocacoes().Where(m => m.Id == id).FirstOrDefault();

            if (moto == null)
            {
                _notificationContext.AddNotification("Moto não encontrada");
                return;
            }
            if(moto.Locacoes.Any(l => l.Ativo))
            {
                _notificationContext.AddNotification("Moto possui locação ativa");
                return;
            }

            _repository.Delete(id);

            _logger.LogInformation($"MotoId:{moto.Id}. Deletado.");
        }
    }
}
