using System;

namespace EscudoNarrador.Dominio.Excecoes
{
    public class RecursoNaoEncontradoExcecao : Exception
    {
        public RecursoNaoEncontradoExcecao() { }
        public RecursoNaoEncontradoExcecao(string menssagem) : base(menssagem)
        {
        }
    }
}
