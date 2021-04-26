using EscudoNarrador.Dominio.Abstracoes.Servicos;
using EscudoNarrador.Entidade;
using Microsoft.Extensions.Logging;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Recursos;
using System.Collections.Generic;

namespace EscudoNarrador.Dominio.Servicos
{
    public class SistemaServico : ServicoCrud<Sistema>, ISistemaServico
    {
        public SistemaServico(IComandoRepositorio<Sistema> comandoRepositorio,
            IConsultaRepositorio<Sistema> consultaRepositorio,
            ILogger<Sistema> log,
            IValidador<Sistema> validador) :
            base(comandoRepositorio, consultaRepositorio, log, validador)
        {
        }

        protected override List<PropriedadeValor> ConfigurarAtualizacoes(Sistema entidade)
        {
            var atualizacoes = PropriedadeValorFabrica<Sistema>.Iniciar()
                  .Add(c => c.Nome, entidade.Nome)
                  .Add(c => c.NomeHigienizado, entidade.NomeHigienizado)
                  .Add(c => c.Abreviacao, entidade.Abreviacao)
                  .Add(c => c.Versoes, entidade.Versoes);

            return atualizacoes.ObterTodos;
        }
    }
}
