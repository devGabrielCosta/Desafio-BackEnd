using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Moq;

namespace UnitTests.Mocks.Repositories
{
    public static class LocacaoRepositoryMock
    {
        public static Mock<ILocacaoRepository> Create()
        {
            return new Mock<ILocacaoRepository>();
        }
        public static void SetupGet(this Mock<ILocacaoRepository> mock, List<Locacao> locacoes)
        {
            mock.Setup(repo => repo.Get()).Returns(locacoes.AsQueryable());
            mock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Guid id) => locacoes.Where(p => p.Id == id).AsQueryable());
        }
        public static void SetupInsertAsync(this Mock<ILocacaoRepository> mock, Locacao locacao)
        {
            mock.Setup(repo => repo.InsertAsync(It.IsAny<Locacao>())).Returns(Task.FromResult(locacao));
        }
    }
}
