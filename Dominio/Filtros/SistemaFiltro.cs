using EscudoNarrador.Entidade;
using EscudoNarrador.Entidade.Extensoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Filtros;

namespace EscudoNarrador.Dominio.Filtros
{
    public class SistemaFiltro : FiltroAbstrato<Sistema>
    {
        public SistemaFiltro(string nome)
        {
            nome = nome.HigienizaString();
            AdicionarRegra(c => c.NomeHigienizado.Contains(nome))
                .SobCondicional(c => !nome.limpoNuloBrancoOuZero());
        }
    }
}
