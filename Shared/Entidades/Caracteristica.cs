using EscudoNarrador.Shared.Enums;

namespace EscudoNarrador.Shared.Entidades
{
    public class Caracteristica
    {
        public string Nome { get; set; }
        public string[] Tags { get; set; }
        public TipoCaracteristica Tipo { get; set; }
        public int Pontos { get; set; }
        public TipoSistema Sistema { get; set; }
    }
}
