using System;

namespace EscudoNarrador.Entidade
{
    public class Sistema
    {
        public Sistema(string nome, string versao, Guid id)
        {
            Nome = nome;
            Versao = versao;
            Id = id;
        }

        public string Nome { get; set; }
        public string Versao { get; set; }
        public Guid Id { get; set; }

        public static Sistema Novo(string nome, string versao)
        {
            return new Sistema(nome, versao, Guid.NewGuid());
        }
    }
}
