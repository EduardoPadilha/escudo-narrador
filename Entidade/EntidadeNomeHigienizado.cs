using EscudoNarrador.Entidade.Extensoes;

namespace EscudoNarrador.Entidade
{
    public abstract class EntidadeNomeHigienizado : Nebularium.Tiamat.Entidades.Entidade
    {
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
    }
}
