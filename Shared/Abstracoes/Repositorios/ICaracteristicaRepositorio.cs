using EscudoNarrador.Shared.Entidades;
using EscudoNarrador.Shared.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscudoNarrador.Shared.Abstracoes.Repositorios
{
    public interface ICaracteristicaRepositorio
    {
        Task<Caracteristica> AdicionarAsync(Caracteristica entidade);
        Task<Caracteristica> ObterAsync(string nome, TipoSistema sistema);
        List<Caracteristica> ObterTodos(string nome, TipoSistema sistema, string[] tags);
        Task DeletarAsync(string nome, TipoSistema sistema);
        Task<Caracteristica> AtualizarAsync(Caracteristica entidade);
    }
}
