using System;

namespace EscudoNarrador.Repositorio.Excecoes
{
    public class UnicidadeException : Exception
    {
        public string Chave { get; set; }
        public UnicidadeException(string chave) : base($"Já existe um registro com essa chave '{chave}'")
        {
            Chave = chave;
        }
    }
}
