using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces.Handlers
{
    public interface ICommandHandler<Tin, Tout>
    {
        Task<Tout> Handle(Tin @event);
    }
    public interface ICommandHandler<Tin>
    {
        Task Handle(Tin @event);
    }
}
