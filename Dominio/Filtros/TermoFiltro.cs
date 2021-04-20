using EscudoNarrador.Entidade;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Filtros;
using System;
using System.Linq;

namespace EscudoNarrador.Dominio.Filtros
{
    public class TermoFiltro : FiltroAbstrato<Termo>
    {
        public TermoFiltro(string sistema, string nome, string[] tags)
        {
            AdicionarRegra(c => c.Sistema.Equals(sistema)).SobCondicional(c => !sistema.limpoNuloBrancoOuZero());
            AdicionarRegra(c => c.NomeHigienizado.Contains(nome)).SobCondicional(c => !nome.LimpoNuloBranco());
            AdicionarRegra(c => tags.All(tag => c.TagsHigienizadas.Contains(tag))).SobCondicional(c => tags.AnySafe());
        }
    }
}
