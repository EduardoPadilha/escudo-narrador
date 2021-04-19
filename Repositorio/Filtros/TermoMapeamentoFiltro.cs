using EscudoNarrador.Repositorio.Mapeamento;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Filtros;
using System;
using System.Linq;

namespace EscudoNarrador.Repositorio.Filtros
{
    public class TermoMapeamentoFiltro : FiltroAbstrato<TermoMapeamento>
    {
        public TermoMapeamentoFiltro(string nome, string[] tags)
        {
            AdicionarRegra(c => c.RowKey.Contains(nome)).SobCondicional(c => !nome.LimpoNuloBranco());
            AdicionarRegra(c => tags.All(tag => c.Tags.Contains(tag))).SobCondicional(c => tags.AnySafe());
        }
    }
}
