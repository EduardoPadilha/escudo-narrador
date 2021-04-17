using EscudoNarrador.Entidade;
using EscudoNarrador.Entidade.Enums;
using EscudoNarrador.Repositorio.Mapeamento;
using Nebularium.Tarrasque.Extensoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Teste.Unidade.Entidades
{
    public class TermoTeste : TesteBase
    {
        public TermoTeste(ITestOutputHelper saida) : base(saida)
        {
        }

        [Fact]
        public void ConveterMapeamento()
        {
            var termo = new Termo
            {
                Sistema = Guid.NewGuid(),
                Nome = "Destreza",
                Tags = new string[] { "Atributos", "Refinamento", "Física" },
                Tipo = TipoTermo.Expansivel | TipoTermo.Ponto,
                Pontos = 0
            };

            var mapeamento = termo.Como<TermoMapeamento>();
            Assert.NotNull(mapeamento);
            Assert.NotNull(mapeamento.PartitionKey);
            Assert.NotNull(mapeamento.RowKey);
        }

        [Fact]
        public void ConveterMapeamentoExpression()
        {
            var nomeQuery = "forca";
            Expression<Func<Termo, bool>> predicado = termo => termo.Nome.Contains(nomeQuery);

            //var predicadoConvertido = predicado.ConvertePredicado<Termo, TermoMapeamento>();
            var predicadoConvertido = predicado.Como<Expression<Func<TermoMapeamento, bool>>>();
            var consulta = Termos.Where(predicadoConvertido);
            var termosResultado = consulta.ToList();
            Assert.NotNull(termosResultado);
            Assert.NotEmpty(termosResultado);
        }


        private static IQueryable<TermoMapeamento> Termos = new List<TermoMapeamento>
        {
            new TermoMapeamento
            {
                PartitionKey = "storytelling",
                RowKey = "destreza",
                Nome = "Destreza",
                TagsHigienizadas = "atributo;refinamento;fisico",
                TagsApresentacao = "Atributo;Refinamento;Físico",
                Tipo = (int)(TipoTermo.Expansivel | TipoTermo.Ponto),
                Pontos = 0
            },
            new TermoMapeamento
            {
                PartitionKey = "storytelling",
                RowKey = "forca",
                Nome = "Força",
                TagsHigienizadas = "atributo;poder;fisico",
                TagsApresentacao = "Atributo;Poder;Físico",
                Tipo = (int)(TipoTermo.Expansivel | TipoTermo.Ponto),
                Pontos = 0
            },
            new TermoMapeamento
            {
                PartitionKey = "storytelling",
                RowKey = "vigor",
                Nome = "Vigor",
                TagsHigienizadas = "atributo;resistencia;fisico",
                TagsApresentacao = "Atributo;Resistência;Físico",
                Tipo = (int)(TipoTermo.Expansivel | TipoTermo.Ponto),
                Pontos = 0
            },
            new TermoMapeamento
            {
                PartitionKey = "storytelling",
                RowKey = "inteligencia",
                Nome = "Inteligência",
                TagsHigienizadas = "atributo;poder;mental",
                TagsApresentacao = "Atributo;Poder;Mental",
                Tipo = (int)(TipoTermo.Expansivel | TipoTermo.Ponto),
                Pontos = 0
            },
             new TermoMapeamento
            {
                PartitionKey = "storytelling",
                RowKey = "autocontrole",
                Nome = "Autocontrole",
                TagsHigienizadas = "atributo;resistencia;social",
                TagsApresentacao = "Atributo;Resistência;Social",
                Tipo = (int)(TipoTermo.Expansivel | TipoTermo.Ponto),
                Pontos = 0
            },
        }.AsQueryable();
    }
}
