using EscudoNarrador.Dominio.Abstracoes;
using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Dominio.Excecoes;
using EscudoNarrador.Dominio.Extensoes;
using EscudoNarrador.Dominio.Validadores;
using EscudoNarrador.Entidade;
using Microsoft.Extensions.Logging;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Validacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscudoNarrador.Dominio.Servicos
{
    public class TermoServico : ITermoServico
    {
        private readonly ITermoRepositorio repositorio;
        private readonly ILogger<Termo> log;
        private readonly ValidadorSimples validadorSimples;
        private readonly IValidador<Termo> validador;

        public TermoServico(ITermoRepositorio repositorio, IValidador<Termo> validador,
            ILogger<Termo> log)
        {
            this.repositorio = repositorio;
            this.log = log;
            validadorSimples = new ValidadorSimples();
            validadorSimples.EventoFalhaValidacao = erros => throw new ValidacaoExcecao(erros);
            this.validador = validador;
            this.validador.EventoFalhaValidacao = erros => throw new ValidacaoExcecao(erros);
        }

        public IEnumerable<Termo> ObterTodos(Guid sistema, string nome, string tags)
        {
            validadorSimples
              .Add(ValidadorPadrao.Guid(nameof(sistema), sistema))
              .Validar();

            nome = nome.HigienizaString();
            var tagsArray = ConverteStringParaArray(tags);
            var resultado = repositorio.ObterTodos(sistema.ToString(), nome, tagsArray);
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

        public async Task<Termo> ObterAsync(Guid sistema, string nome)
        {
            ValidarEntradas(sistema, nome);

            nome = nome.HigienizaString();
            var resultado = await repositorio.ObterAsync(sistema.ToString(), nome);
            return resultado;
        }

        public async Task<Termo> AdicionarAsync(Termo termo)
        {
            validador.Validar(termo);
            var resultado = await repositorio.AdicionarAsync(termo);
            return resultado;
        }

        public async Task<Termo> AtualizarAsync(Termo termo)
        {
            validador.Validar(termo);
            var resultado = await repositorio.AtualizarAsync(termo);
            return resultado;
        }
        public async Task DeletarAsync(Guid sistema, string nome)
        {
            ValidarEntradas(sistema, nome);

            nome = nome.HigienizaString();
            await repositorio.DeletarAsync(sistema.ToString(), nome);
        }

        private void ValidarEntradas(Guid sistema, string nome)
        {
            validadorSimples
               .Add(ValidadorPadrao.Guid(nameof(sistema), sistema))
               .Add(ValidadorPadrao.CampoVazio(nameof(nome), nome))
               .Validar();
        }
    }
}
