﻿using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using Dominio.Services;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Fixtures;
using UnitTests.Mocks;
using UnitTests.Mocks.Repositories;
using UnitTests.Mocks.Services;

namespace UnitTests.Services
{
    public class LocacaoServiceTests
    {
        private Mock<ILocacaoRepository> _locacaoRepositoryMock;
        private Mock<IEntregadorService> _entregadorServiceMock;
        private Mock<IMotoService> _motoServiceMock;
        private Mock<INotificationContext> _notificationContextMock;
        private Mock<ILogger<LocacaoService>> _loggerMock;

        public LocacaoServiceTests()
        {
            _locacaoRepositoryMock = LocacaoRepositoryMock.Create();
            _entregadorServiceMock = EntregadorServiceMock.Create();
            _motoServiceMock = MotoServiceMock.Create();
            _notificationContextMock = NotificationContextMock.Create();
            _loggerMock = LoggerMock.Create<LocacaoService>();
        }

        [Fact]
        public async Task InsertLocacaoAsync_MotoDisponivelEEntregadorExiste_Sucesso()
        {
            // Arrange
            var entregadores = EntregadorFixture.CreateList(1);
            var entregador = EntregadorFixture.Create("a");
            entregadores.Add(entregador);

            var locacao = LocacaoFixture.Create();
            locacao.EntregadorId = entregador.Id;

            var motos = MotoFixture.CreateList(1);

            _motoServiceMock.SetupGetMotosDisponiveis(motos);

            var logLevel = LogLevel.Information;
            _loggerMock.SetupLogLevel(logLevel);

            _locacaoRepositoryMock.SetupInsertAsync(locacao);
            _entregadorServiceMock.SetupGetLocacoes(entregadores);

            var locacaoService = new LocacaoService(_locacaoRepositoryMock.Object,
                                                    _entregadorServiceMock.Object,
                                                    _motoServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await locacaoService.InsertLocacaoAsync(locacao);

            // Assert
            _motoServiceMock.Verify(ms => ms.GetMotosDisponiveis(), Times.Once());
            _notificationContextMock.Verify(nc => nc.AddNotification(It.IsAny<string>()), Times.Never);
            _locacaoRepositoryMock.Verify(lr => lr.InsertAsync(locacao), Times.Once());

        }

        [Fact]
        public async Task InsertLocacaoAsync_NenhumaMotoDisponivel_NotificaENaoInsere()
        {
            // Arrange
            var locacao = LocacaoFixture.Create();
            _motoServiceMock.SetupGetMotosDisponiveis(new List<Moto>());

            var logLevel = LogLevel.Information;
            _loggerMock.SetupLogLevel(logLevel);

            var locacaoService = new LocacaoService(_locacaoRepositoryMock.Object,
                                                    _entregadorServiceMock.Object,
                                                    _motoServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await locacaoService.InsertLocacaoAsync(locacao);

            // Assert
            _motoServiceMock.Verify(ms => ms.GetMotosDisponiveis(), Times.Once());
            _notificationContextMock.Verify(nc => nc.AddNotification("Nenhuma moto disponivel"), Times.Once);
        }

        [Fact]
        public async Task InsertLocacaoAsync_EntregadorNaoExiste_NotificaENaoInsere()
        {
            // Arrange
            var locacao = LocacaoFixture.Create();
            var entregadorId = locacao.EntregadorId;

            _motoServiceMock.SetupGetMotosDisponiveis(new List<Moto> { MotoFixture.Create() });
            _entregadorServiceMock.SetupGetLocacoes(new List<Entregador>());

            var locacaoService = new LocacaoService(_locacaoRepositoryMock.Object,
                                                    _entregadorServiceMock.Object,
                                                    _motoServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await locacaoService.InsertLocacaoAsync(locacao);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification("Entregador não existe"), Times.Once);
            _entregadorServiceMock.Verify(r => r.GetLocacoes(entregadorId), Times.Once);
            _locacaoRepositoryMock.Verify(r => r.InsertAsync(locacao), Times.Never);
        }

        [Fact]
        public async Task InsertLocacaoAsync_EntregadorPossuiLocacaoAtiva_NotificaENaoInsere()
        {
            // Arrange
            var locacaoAtiva = LocacaoFixture.Create();
            var locacao = LocacaoFixture.Create();
            var entregador = EntregadorFixture.Create();

            entregador.Locacoes.Add(locacaoAtiva);
            locacao.EntregadorId = entregador.Id;

            _motoServiceMock.SetupGetMotosDisponiveis(new List<Moto> { MotoFixture.Create() });
            _entregadorServiceMock.SetupGetLocacoes(new List<Entregador> { entregador });

            var locacaoService = new LocacaoService(_locacaoRepositoryMock.Object,
                                                    _entregadorServiceMock.Object,
                                                    _motoServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await locacaoService.InsertLocacaoAsync(locacao);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification("Entregador já possui uma locação ativa"), Times.Once);
            _locacaoRepositoryMock.Verify(r => r.InsertAsync(locacao), Times.Never);
        }

        [Fact]
        public async Task InsertLocacaoAsync_EntregadorNaoPossuiCategoriaA_NotificaENaoInsere()
        {
            // Arrange
            var locacao = LocacaoFixture.Create();
            var entregador = EntregadorFixture.Create();

            entregador.CnhTipo = "B";
            locacao.EntregadorId = entregador.Id;

            _motoServiceMock.SetupGetMotosDisponiveis(new List<Moto> { MotoFixture.Create() });
            _entregadorServiceMock.SetupGetLocacoes(new List<Entregador> { entregador });

            var locacaoService = new LocacaoService(_locacaoRepositoryMock.Object,
                                                    _entregadorServiceMock.Object,
                                                    _motoServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            await locacaoService.InsertLocacaoAsync(locacao);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification("Entregador não possui categoria A"), Times.Once);
            _locacaoRepositoryMock.Verify(r => r.InsertAsync(locacao), Times.Never);
        }


        [Fact]
        public void ConsultarDevolucao_LocacaoNaoEncontrada_NotificaENaoRetornaPreco()
        {
            // Arrange
            var locacao = LocacaoFixture.Create();
            _locacaoRepositoryMock.SetupGet(new List<Locacao>());

            var locacaoService = new LocacaoService(_locacaoRepositoryMock.Object,
                                                    _entregadorServiceMock.Object,
                                                    _motoServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            var result = locacaoService.ConsultarDevolucao(locacao.Id, DateTime.Now);

            // Assert
            _notificationContextMock.Verify(nc => nc.AddNotification("Locação não encontrada"), Times.Once);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ConsultarDevolucao_LocacaoEncontrada_Sucesso()
        {
            // Arrange
            var previsaoDevolucao = DateTime.Now.AddDays(7);
            var locacao = LocacaoFixture.Create(Plano.A);

            _locacaoRepositoryMock.SetupGet(new List<Locacao> { locacao });

            var locacaoService = new LocacaoService(_locacaoRepositoryMock.Object,
                                                    _entregadorServiceMock.Object,
                                                    _motoServiceMock.Object,
                                                    _notificationContextMock.Object,
                                                    _loggerMock.Object);

            // Act
            var preco = locacaoService.ConsultarDevolucao(locacao.Id, previsaoDevolucao);

            // Assert
            Assert.Equal(210, preco);
        }

    }

}