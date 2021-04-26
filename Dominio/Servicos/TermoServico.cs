using EscudoNarrador.Dominio.Abstracoes.Servicos;
using EscudoNarrador.Dominio.Validadores;
using EscudoNarrador.Entidade;
using Microsoft.Extensions.Logging;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Recursos;
using System.Collections.Generic;

namespace EscudoNarrador.Dominio.Servicos
{
    public class TermoServico : ServicoCrud<Termo>, ITermoServico
    {
        public TermoServico(IComandoRepositorio<Termo> comandoRepositorio,
            IConsultaRepositorio<Termo> consultaRepositorio,
            ILogger<Termo> log, IValidador<Termo> validador) :
            base(comandoRepositorio, consultaRepositorio, log, validador)
        {
        }

        protected override string[] AdicionarRulerset => new string[] { TermoValidador.AO_ADICIONAR };
        protected override string[] AtualizarRulerset => new string[] { TermoValidador.AO_ATUALIZAR };

        protected override List<PropriedadeValor> ConfigurarAtualizacoes(Termo termo)
        {
            var atualizacoes = PropriedadeValorFabrica<Termo>.Iniciar()
                 .Add(c => c.Nome, termo.Nome)
                 .Add(c => c.NomeHigienizado, termo.NomeHigienizado)
                 .Add(c => c.Tags, termo.Tags)
                 .Add(c => c.TagsHigienizadas, termo.TagsHigienizadas)
                 .Add(c => c.Tipo, termo.Tipo)
                 .Add(c => c.Descricao, termo.Descricao)
                 .Add(c => c.Pontos, termo.Pontos);

            return atualizacoes.ObterTodos;
        }
    }
}
