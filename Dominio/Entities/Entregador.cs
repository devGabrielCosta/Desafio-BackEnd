﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Entregador : BaseEntity
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cnh { get; set; }
        public string CnhTipo { get; set; }
        public string? CnhImagem { get; set; }

        public virtual ICollection<Locacao> Locacoes { get; set; } = new List<Locacao>();
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public virtual ICollection<Pedido> Notificacoes { get; set; } = new List<Pedido>();

        public Entregador(string nome, string cnpj, DateTime dataNascimento, string cnh, string cnhTipo) : base()
        {
            Nome = nome;
            Cnpj = cnpj;
            DataNascimento = dataNascimento;
            Cnh = cnh;
            CnhTipo = cnhTipo;
        }
    }
}
