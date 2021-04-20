using EscudoNarrador.Dominio.Abstracoes;
using EscudoNarrador.Dominio.Abstracoes.Repositorios;
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
using System.Linq;
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

        public Task<IEnumerable<Termo>> ObterTodosAsync(string sistema, string nome, string tags)
        {
            validadorSimples
              .Add(ValidadorPadrao.CampoVazio(nameof(sistema), sistema))
              .Validar();

            nome = nome.HigienizaString();
            var tagsArray = ConverteStringParaArray(tags);
            var resultado = consultaRepositorio.ObterTodosAtivosAsync(new TermoFiltro(sistema, nome, tagsArray));
            return resultado;
        }

        private string[] ConverteStringParaArray(string tags)
        {
            if (string.IsNullOrWhiteSpace(tags))
                return null;
            var separador = tags?.IndexOf(";") > 0 ? ";" : ",";
            var tagsHigienizadas = tags?.Split(separador)?.Select(c => c.HigienizaString());
            return tagsHigienizadas?.ToArray();
        }

        public async Task<Termo> ObterAsync(string sistema, string nome)
        {
            ValidarEntradas(sistema, nome);

            nome = nome.HigienizaString();
            var resultado = await consultaRepositorio.ObterTodosAtivosAsync(termo => termo.Sistema == sistema
                                                                        && termo.NomeHigienizado == nome);
            return resultado?.FirstOrDefault();
        }

        public async Task<Termo> AdicionarAsync(Termo termo)
        {
            validador.Validar(termo);
            await comandoRepositorio.AdicionarAsync(termo);
            return termo;
        }

        public async Task<Termo> AtualizarAsync(Termo termo)
        {
            validador.Validar(termo);
            var atualizacoes = PropriedadeValorFabrica<Termo>.Iniciar()
                 .Add(c => c.Tags, termo.Tags)
                 .Add(c => c.Tipo, termo.Tipo)
                 .Add(c => c.Pontos, termo.Pontos);
            var resultado = await comandoRepositorio
                .AtualizarUmAsync(termo => termo.Sistema == termo.Sistema && termo.Nome == termo.Nome, atualizacoes.ObterTodos);
            if (resultado)
                throw new Exception("Não foi possível atualizar. Para mais informações contate o administrador");

            return termo;
        }
        public async Task DeletarAsync(string sistema, string nome)
        {
            ValidarEntradas(sistema, nome);

            nome = nome.HigienizaString();
            var termo = await ObterAsync(sistema, nome);
            if (termo != null)
            {
                var resultado = await comandoRepositorio.RemoverUmAsync(termo.Id);
                if (resultado) return;
            }

            throw new Exception("Não foi possível deletar. Para mais informações contate o administrador");
        }

        private void ValidarEntradas(string sistema, string nome)
        {
            validadorSimples
               .Add(ValidadorPadrao.CampoVazio(nameof(sistema), sistema))
               .Add(ValidadorPadrao.CampoVazio(nameof(nome), nome))
               .Validar();
        }
    }
}
