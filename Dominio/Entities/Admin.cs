using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Admin
    {
        public Guid Id { get; set; }
      
        public Admin()
        {
            Id = Guid.NewGuid();
        }
    }
}
