using EscudoNarrador.Entidade;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscudoNarrador.Dominio.Abstracoes
{
    public interface ITermoServico
    {
        Task<IEnumerable<Termo>> ObterTodosAsync(string sistema, string nome, string tags);
        Task<Termo> ObterAsync(string sistema, string nome);
        Task<Termo> AdicionarAsync(Termo termo);
        Task<Termo> AtualizarAsync(Termo termo);
        Task DeletarAsync(string sistema, string nome);
    }
}
