using EscudoNarrador.Entidade.Enums;
using System;

namespace EscudoNarrador.Entidade
{
    public class Termo
    {
        public Guid Sistema { get; set; }
        public string Nome { get; set; }
        public string[] Tags { get; set; }
        public TipoTermo Tipo { get; set; }
        public int Pontos { get; set; }
    }
}
