using EscudoNarrador.Entidade;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscudoNarrador.Dominio.Abstracoes
{
    public interface ITermoServico
    {
        Task<IEnumerable<Termo>> ObterTodosAsync(string sistema, string query);
        Task<Termo> ObterAsync(string id);
        Task<Termo> AdicionarAsync(Termo termo);
        Task<Termo> AtualizarAsync(Termo termo);
        Task DeletarAsync(string id);
    }
}
