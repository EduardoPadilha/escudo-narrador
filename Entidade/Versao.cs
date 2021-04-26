using EscudoNarrador.Entidade.Abstracoes;
using EscudoNarrador.Entidade.Extensoes;
using System;
using System.Collections.Generic;

namespace EscudoNarrador.Entidade
{
    public class Versao : IEntidadeComGuidIdAutogerenciada
    {
        public Guid Id { get; set; }
        private string nome;
        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                NomeHigienizado = nome.HigienizaString();
            }
        }
        public string NomeHigienizado { get; set; }
        public string Abreviacao { get; set; }
        public List<Versao> Suplementos { get; set; }
    }
}
