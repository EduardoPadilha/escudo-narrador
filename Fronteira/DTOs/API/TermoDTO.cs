using EscudoNarrador.Entidade.Enums;

namespace EscudoNarrador.Fronteira.DTOs.API
{
    public class TermoDTO
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string[] Tags { get; set; }
        public TipoTermo Tipo { get; set; }
        public int Pontos { get; set; }
        public string Descricao { get; set; }
        public string Sistema { get; set; }
    }
}
