using System;

namespace EscudoNarrador.Repositorio.Excecoes
{
    public class RecursoNaoEncontradoException : Exception
    {
        public RecursoNaoEncontradoException() { }
        public RecursoNaoEncontradoException(string menssagem) : base(menssagem)
        {

        }
    }
}
