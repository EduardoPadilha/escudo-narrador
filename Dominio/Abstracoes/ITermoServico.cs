using EscudoNarrador.Entidade;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscudoNarrador.Dominio.Abstracoes
{
    public interface ITermoServico
    {
        IEnumerable<Termo> ObterTodos(Guid sistema, string nome, string tags);
        Task<Termo> ObterAsync(Guid sistema, string nome);
        Task<Termo> AdicionarAsync(Termo termo);
        Task<Termo> AtualizarAsync(Termo termo);
        Task DeletarAsync(Guid sistema, string nome);
    }
}
