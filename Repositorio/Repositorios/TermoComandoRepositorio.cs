using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Entidade;
using Microsoft.Extensions.Logging;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Repositorios;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Excecoes;
using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EscudoNarrador.Repositorio.Repositorios
{
    public class TermoComandoRepositorio : ComandoRepositorio<Termo> , ITermoComandoRepositorio
    {
        private readonly ITermoConsultaRepositorio consultaRepositorio;

        public TermoComandoRepositorio(IMongoContexto contexto,
            ITermoConsultaRepositorio consultaRepositorio,
            ILogger<Termo> logger) : base(contexto, logger)
        {
            this.consultaRepositorio = consultaRepositorio;
        }

        protected async override Task ValidaUnicidade(Termo entidade)
        {
            var ativosComMesmaChave = await consultaRepositorio.ObterTodosAtivosAsync(c => c.Nome == entidade.Nome && c.Id != entidade.Id);

            if (ativosComMesmaChave.AnySafe())
                throw new UnicidadeExcecao(entidade.Nome);
        }

        protected async override Task ValidaUnicidadeAtualizacao(Expression<Func<Termo, bool>> predicado, List<PropriedadeValor> propriedades)
        {
            if (!propriedades.Any(c => c.Nome == nameof(Termo.Nome))) return;
            var mutaveis = await consultaRepositorio.ObterTodosAtivosAsync(predicado);
            if (!mutaveis.AnySafe()) return;

            var valor = (string)propriedades.FirstOrDefault(c => c.Nome == nameof(Termo.Nome)).Valor;

            if (mutaveis.Count() > 1)
                throw new UnicidadeExcecao(valor);

            var comUnicidade = await consultaRepositorio.ObterTodosAtivosAsync(c => c.Nome == valor);

            if (comUnicidade.Count() > 1) throw new UnicidadeExcecao(valor);

            if (comUnicidade.Count() > 0 && !comUnicidade.Any(c => mutaveis.FirstOrDefault().Id == c.Id))
                throw new UnicidadeExcecao(valor);
        }
    }
}
