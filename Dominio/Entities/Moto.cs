using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Moto : BaseEntity
    {
        public Moto(int ano, string modelo, string placa) : base ()
        {
            Ano = ano;
            Modelo = modelo;
            Placa = placa;
            Disponivel = true;
        }

        public int Ano { get; set; }
        public string Modelo {  get; set; }
        public string Placa { get; set; }
        public bool Disponivel { get; set; }

        public virtual ICollection<Locacao> Locacoes { get; set; } = new List<Locacao>();
    }
}
