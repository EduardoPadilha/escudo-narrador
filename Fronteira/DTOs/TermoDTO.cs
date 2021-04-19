using EscudoNarrador.Entidade.Enums;
using System;

namespace EscudoNarrador.Fronteira.DTOs
{
    public class TermoDTO
    {
        public TermoDTO() { }
        public string Nome { get; set; }
        public string[] Tags { get; set; }
        public TipoTermo Tipo { get; set; }
        public int Pontos { get; set; }
    }
}
