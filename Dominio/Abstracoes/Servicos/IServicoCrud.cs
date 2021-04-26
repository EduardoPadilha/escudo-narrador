using Nebularium.Tiamat.Abstracoes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscudoNarrador.Dominio.Abstracoes.Servicos
{
    public interface IServicoCrud<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<IEnumerable<TEntidade>> ObterTodosAsync(IFiltro<TEntidade> filtro);
        Task<TEntidade> ObterAsync(string id);
        Task<TEntidade> AdicionarAsync(TEntidade entidade);
        Task<TEntidade> AtualizarAsync(TEntidade entidade);
        Task DeletarAsync(string id);
    }
}
