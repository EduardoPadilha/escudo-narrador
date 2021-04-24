using EscudoNarrador.Entidade;
using EscudoNarrador.Entidade.Extensoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EscudoNarrador.Dominio.Filtros
{
    public class TermoFiltro : FiltroAbstrato<Termo>
    {
        public TermoFiltro(string sistema, string query)
        {
            AdicionarRegra(c => c.Sistema.Equals(sistema))
                .SobCondicional(c => !sistema.limpoNuloBrancoOuZero());

            var expressoes = SeparaExpressoesChave(query);
            if (!expressoes.AnySafe()) return;

            var nomes = ObterNomes(expressoes);
            var tags = ObterTags(expressoes);
            var descricoes = ObterDescricoes(expressoes);

            AdicionarRegra(c => nomes.Contains(c.NomeHigienizado)).SobCondicional(c => nomes.AnySafe());

            foreach (var tag in tags)
                AdicionarRegra(c => c.TagsHigienizadas.Contains(tag)).SobCondicional(c => true);

            foreach (var descricao in descricoes)
                AdicionarRegra(c => c.Descricao.ToLower().Contains(descricao.ToLower())).SobCondicional(c => true);


        }
        private IEnumerable<string> SeparaExpressoesChave(string query)
        {
            if (query.LimpoNuloBranco()) return null;

            var matches = Regex.Matches(query, @"[@#]?[\""\''].+?[\""\'']|[^ ]+");
            return matches.Select(c => c.Value);
        }
        private IEnumerable<string> ObterNomes(IEnumerable<string> query)
        {
            return ObterTodosComecandoCom(query, '@');
        }

        private IEnumerable<string> ObterTags(IEnumerable<string> query)
        {
            return ObterTodosComecandoCom(query, '#');
        }

        private IEnumerable<string> ObterDescricoes(IEnumerable<string> query)
        {
            var descricoes = query.Where(expressao => !expressao.StartsWith('#') && !expressao.StartsWith('@'));
            var descricoesHigienizadas = descricoes?.Select(c => c.Replace("\"", string.Empty).Replace("'", string.Empty));
            return descricoesHigienizadas;
        }

        private IEnumerable<string> ObterTodosComecandoCom(IEnumerable<string> query, char caractere)
        {
            var expressoes = query.Where(expressao => expressao.StartsWith(caractere));
            var expressoesHigienizadas = expressoes?.Select(c => c.Replace(caractere.ToString(), string.Empty)?.HigienizaString());
            return expressoesHigienizadas;
        }
    }
}
