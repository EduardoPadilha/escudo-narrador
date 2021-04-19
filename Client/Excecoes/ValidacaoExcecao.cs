using Client.VOs;
using System;
using System.Collections.Generic;

namespace Client.Excecoes
{
    public class ValidacaoExcecao : Exception
    {
        public IEnumerable<ErroValidacao> Erros { get; protected set; }

        public ValidacaoExcecao(IEnumerable<ErroValidacao> erros)
        {
            Erros = erros;
        }
    }
}
