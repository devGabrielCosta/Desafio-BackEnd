using Dominio.Entities;
using Dominio.Interfaces.Services;
using Moq;

namespace UnitTests.Mocks.Services
{
    public static class EntregadorServiceMock
    {
        public static Mock<IEntregadorService> Create()
        {
            return new Mock<IEntregadorService>();
        }

        public static void SetupGet(this Mock<IEntregadorService> mock, List<Entregador> entregadores)
        {
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => entregadores.FirstOrDefault(p => p.Id == id));
        }

        public static void SetupGetLocacoes(this Mock<IEntregadorService> mock, List<Entregador> entregadores)
        {
            mock.Setup(repo => repo.GetLocacoes(It.IsAny<Guid>())).Returns((Guid id) => entregadores.FirstOrDefault(p => p.Id == id));
        }
    }
}
