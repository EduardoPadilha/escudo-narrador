using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EscudoNarrador.Shared.Abstracoes
{
    public interface IRepositorio<T> where T : class
    {
        Task<T> AdicionarAsync(T entidade);
        Task<T> AtualizarAsync(T entidade);
        Task DeletarAsync(string chaveParticao, string chaveLinha);
        Task<T> ObterAsync(string chaveParticao, string chaveLinha);
        List<T> ObterTodos<TMapeamento>(string chaveParticao, Func<TMapeamento, bool> filtros);
    }
}
