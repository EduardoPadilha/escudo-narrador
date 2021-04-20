using EscudoNarrador.Entidade.Enums;
using EscudoNarrador.Entidade.Extensoes;
using System.Linq;

namespace EscudoNarrador.Entidade
{
    public class Termo : Nebularium.Tiamat.Entidades.Entidade
    {
        public string Sistema { get; set; }
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
        private string[] tags;
        public string[] Tags
        {
            get => tags;
            set
            {
                tags = value;
                TagsHigienizadas = tags?.Select(c => c.HigienizaString())?.ToArray();
            }
        }
        public string[] TagsHigienizadas { get; set; }
        public TipoTermo Tipo { get; set; }
        public int Pontos { get; set; }
    }
}
