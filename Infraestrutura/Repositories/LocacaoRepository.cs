using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;

namespace Infraestrutura.Repositories
{
    public class LocacaoRepository : BaseRepository<Locacao> , ILocacaoRepository
    {
        public LocacaoRepository(AppDbContext context) : base(context)
        { 
        }
    }
}
