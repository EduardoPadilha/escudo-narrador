using System;

namespace EscudoNarrador.Shared.Entidades
{
    public class Caracteristica
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string[] Tags { get; set; }
        public TipoCaracteristica Tipo { get; set; }
        public int Pontos { get; set; }
    }

    [Flags]
    public enum TipoCaracteristica
    {
        Ponto = 1,
        Expansivel = 2,
        Fixo = 4,
        Consumivel = 16,
        Informativa = 32
    }
}
