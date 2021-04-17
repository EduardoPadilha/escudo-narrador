using Nebularium.Tiamat.Validacoes;
using System;
using System.Collections.Generic;

namespace EscudoNarrador.Dominio.Excecoes
{
    public class ValidacaoExcecao : Exception
    {
        public List<ErroValidacao> Erros { get; protected set; }

        public ValidacaoExcecao(List<ErroValidacao> erros)
        {
            Erros = erros;
        }

        public ValidacaoExcecao(List<ValidacaoSimples> erros)
        {
            Erros = erros.ConvertAll(c => new ErroValidacao { NomePropriedade = c.Campo, Mensagem = c.Mensagem });
        }
    }
}
