using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Entidade;
using EscudoNarrador.Repositorio.Abstracoes;
using EscudoNarrador.Repositorio.Filtros;
using EscudoNarrador.Repositorio.Mapeamento;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EscudoNarrador.Repositorio.Repositorios
{
    public class TermoRepositorio :
        RepositorioAbstrato<Termo, TermoMapeamento>,
        ITermoRepositorio
    {
        public TermoRepositorio(ICosmosTableContexto contexto, ILogger<TermoMapeamento> log) :
            base(contexto, log)
        {
        }
        public IEnumerable<Termo> ObterTodos(string chaveParticao, string nome, string[] tags)
        {
            return ObterTodos(new TermoMapeamentoFiltro(chaveParticao, nome, tags));
        }

        protected override string NomeTabela => "Termos";
    }
}
