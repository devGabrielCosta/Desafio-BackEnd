using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositories
{
    public class EntregadorRepository : BaseRepository<Entregador> , IEntregadorRepository
    {
        public EntregadorRepository(AppDbContext context) : base(context)
        { 
        }

        public IQueryable<Entregador> GetLocacoes()
        {
            return _context.Set<Entregador>().Include("Locacoes");
        }

        public IQueryable<Entregador> EntregadoresAptosPedido()
        {
            var entregador = _context.Set<Entregador>().Include("Locacoes");
            var entregadorComLocacao = entregador.Where(e => e.Locacoes.Any(l => l.Ativo)).Include("Pedidos");
            var entregadorSemPedido = entregadorComLocacao.Where(e => !e.Pedidos.Any(p => p.Situacao == Situacao.Aceito));
            return entregadorSemPedido;
        }
    }
}
