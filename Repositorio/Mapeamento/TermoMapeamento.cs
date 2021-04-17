using Microsoft.Azure.Cosmos.Table;

namespace EscudoNarrador.Repositorio.Mapeamento
{
    public class TermoMapeamento : TableEntity
    {
        public string Nome { get; set; }
        public string TagsHigienizadas { get; set; }
        public string TagsApresentacao { get; set; }
        public string[] Tags => TagsHigienizadas.Split(';');
        public int Tipo { get; set; }
        public int Pontos { get; set; }
    }
}
