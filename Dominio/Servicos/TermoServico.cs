using EscudoNarrador.Dominio.Abstracoes;
using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Dominio.Excecoes;
using EscudoNarrador.Dominio.Filtros;
using EscudoNarrador.Dominio.Validadores;
using EscudoNarrador.Entidade;
using EscudoNarrador.Entidade.Extensoes;
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
    public class TermoServico : ITermoServico
    {
        private readonly ITermoComandoRepositorio comandoRepositorio;
        private readonly ITermoConsultaRepositorio consultaRepositorio;
        private readonly ILogger<Termo> log;
        private readonly ValidadorSimples validadorSimples;
        private readonly IValidador<Termo> validador;

        public TermoServico(ITermoComandoRepositorio comandoRepositorio,
            ITermoConsultaRepositorio consultaRepositorio,
            IValidador<Termo> validador,
            ILogger<Termo> log)
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

        public async Task<IEnumerable<Termo>> ObterTodosAsync(string sistema, string query)
        {
            ValidarCampoVazio(nameof(Termo.Sistema), sistema);

            var resultado = await consultaRepositorio.ObterTodosAtivosAsync(new TermoFiltro(sistema, query));
            return resultado;
        }

        public async Task<Termo> ObterAsync(string id)
        {
            ValidarCampoVazio(nameof(Termo.Id), id);

            id = id.HigienizaString();
            var resultado = await consultaRepositorio.ObterAtivoAsync(id);
            return resultado ?? throw new RecursoNaoEncontradoExcecao("Termo não encontrado");
        }

        public async Task<Termo> AdicionarAsync(Termo termo)
        {
            validador.Validar(termo, TermoValidador.AO_ADICIONAR);
            await comandoRepositorio.AdicionarAsync(termo);
            return termo;
        }

        public async Task<Termo> AtualizarAsync(Termo termo)
        {
            validador.Validar(termo, TermoValidador.AO_ATUALIZAR);
            var atualizacoes = PropriedadeValorFabrica<Termo>.Iniciar()
                 .Add(c => c.Nome, termo.Nome)
                 .Add(c => c.NomeHigienizado, termo.NomeHigienizado)
                 .Add(c => c.Tags, termo.Tags)
                 .Add(c => c.TagsHigienizadas, termo.TagsHigienizadas)
                 .Add(c => c.Tipo, termo.Tipo)
                 .Add(c => c.Descricao, termo.Descricao)
                 .Add(c => c.Pontos, termo.Pontos);
            var resultado = await comandoRepositorio
                .AtualizarUmAsync(t => t.Id == termo.Id, atualizacoes.ObterTodos);
            //if (resultado)
            //    throw new Exception("Não foi possível atualizar. Para mais informações contate o administrador");

            return termo;
        }
        public async Task DeletarAsync(string id)
        {
            ValidarCampoVazio(nameof(Termo.Id), id);

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
