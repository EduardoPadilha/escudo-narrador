using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Validacoes;
using System;

namespace EscudoNarrador.Dominio.Validadores
{
    public static class ValidadorPadrao
    {
        public static Func<string, string, ValidacaoSimples> CampoVazio =
            (nome, valor) => new ValidacaoSimples(nome, "Campo não pode ser nulo ou vazio", () => !valor.LimpoNuloBranco());

        public static Func<string, Guid, ValidacaoSimples> Guid = (nome, guid) =>
                new ValidacaoSimples(nome, "Campo não pode ser nulo ou vazio", () => guid != default);
    }
}
