using EscudoNarrador.Dominio.Abstracoes.Servicos;
using EscudoNarrador.Dominio.Excecoes;
using EscudoNarrador.Dominio.Validadores;
using Microsoft.Extensions.Logging;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Excecoes;
using Nebularium.Tiamat.Recursos;
using Nebularium.Tiamat.Validacoes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscudoNarrador.Dominio.Servicos
{
    public abstract class ServicoCrud<TEntidade> : IServicoCrud<TEntidade> where TEntidade : Nebularium.Tiamat.Entidades.Entidade, new()
    {
        protected readonly IComandoRepositorio<TEntidade> comandoRepositorio;
        protected readonly IConsultaRepositorio<TEntidade> consultaRepositorio;
        protected readonly ILogger<TEntidade> log;
        protected readonly ValidadorSimples validadorSimples;
        protected readonly IValidador<TEntidade> validador;

        protected ServicoCrud(IComandoRepositorio<TEntidade> comandoRepositorio,
            IConsultaRepositorio<TEntidade> consultaRepositorio,
            ILogger<TEntidade> log,
            IValidador<TEntidade> validador)
        {
            this.comandoRepositorio = comandoRepositorio;
            this.consultaRepositorio = consultaRepositorio;
            this.log = log;
            validadorSimples = new ValidadorSimples
            {
                EventoFalhaValidacao = erros => throw new ValidacaoExcecao(erros)
            };
            this.validador = validador;
            this.validador.EventoFalhaValidacao = erros => throw new ValidacaoExcecao(erros);
        }

        protected virtual string[] AdicionarRulerset => null;
        protected virtual string[] AtualizarRulerset => null;
        protected abstract List<PropriedadeValor> ConfigurarAtualizacoes(TEntidade entidade);

        public virtual async Task<IEnumerable<TEntidade>> ObterTodosAsync(IFiltro<TEntidade> filtro)
        {
            var resultado = await consultaRepositorio.ObterTodosAtivosAsync(filtro);
            return resultado;
        }

        public virtual async Task<TEntidade> ObterAsync(string id)
        {
            ValidarCampoVazio(nameof(Nebularium.Tiamat.Entidades.Entidade.Id), id);
            var resultado = await consultaRepositorio.ObterAtivoAsync(id);
            return resultado ?? throw new RecursoNaoEncontradoExcecao("Registro não encontrado");
        }

        public virtual async Task<TEntidade> AdicionarAsync(TEntidade entidade)
        {
            validador.Validar(entidade, AdicionarRulerset);
            await comandoRepositorio.AdicionarAsync(entidade);
            return entidade;
        }

        public virtual async Task<TEntidade> AtualizarAsync(TEntidade entidade)
        {
            validador.Validar(entidade, AtualizarRulerset);

            var atualizacoes = ConfigurarAtualizacoes(entidade);
            var resultado = await comandoRepositorio
                .AtualizarUmAsync(t => t.Id == entidade.Id, atualizacoes);

            return entidade;
        }

        public virtual async Task DeletarAsync(string id)
        {
            ValidarCampoVazio(nameof(Nebularium.Tiamat.Entidades.Entidade.Id), id);

            var resultado = await comandoRepositorio.AtivarDesativarUmAsync(id, false);
            if (resultado) return;

            throw new Exception("Não foi possível deletar. Para mais informações contate o administrador");
        }

        private void ValidarCampoVazio(string nome, string valor)
        {
            validadorSimples
               .Add(ValidadorPadrao.CampoVazio(nome, valor))
               .Validar();
        }
    }
}
