﻿using EscudoNarrador.Dominio.Abstracoes.Repositorios;
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
    public class SistemaComandoRepositorio : ComandoRepositorio<Sistema>, ISistemaComandoRepositorio
    {
        private readonly ISistemaConsultaRepositorio consultaRepositorio;

        public SistemaComandoRepositorio(IMongoContexto contexto,
            ISistemaConsultaRepositorio consultaRepositorio,
            ILogger<Sistema> logger) : base(contexto, logger)
        {
            this.consultaRepositorio = consultaRepositorio;
        }

        protected async override Task ValidaUnicidade(Sistema entidade)
        {
            var ativosComMesmaChave = await consultaRepositorio
                .ObterTodosAtivosAsync(c => c.NomeHigienizado == entidade.NomeHigienizado && c.Id != entidade.Id);

            if (ativosComMesmaChave.AnySafe())
                throw new UnicidadeExcecao(entidade.NomeHigienizado);
        }

        protected async override Task ValidaUnicidadeAtualizacao(Expression<Func<Sistema, bool>> predicado, List<PropriedadeValor> propriedades)
        {
            if (!propriedades.Any(c => c.Nome == nameof(Sistema.NomeHigienizado))) return;
            var mutaveis = await consultaRepositorio.ObterTodosAtivosAsync(predicado);
            if (!mutaveis.AnySafe()) return;

            var valor = (string)propriedades.FirstOrDefault(c => c.Nome == nameof(Sistema.NomeHigienizado)).Valor;

            if (mutaveis.Count() > 1)
                throw new UnicidadeExcecao(valor);

            var comUnicidade = await consultaRepositorio.ObterTodosAtivosAsync(c => c.NomeHigienizado == valor);

            if (comUnicidade.Count() > 1) throw new UnicidadeExcecao(valor);

            if (comUnicidade.Count() > 0 && !comUnicidade.Any(c => mutaveis.FirstOrDefault().Id == c.Id))
                throw new UnicidadeExcecao(valor);
        }
    }
}
