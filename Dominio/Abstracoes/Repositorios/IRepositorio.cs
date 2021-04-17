using Nebularium.Tiamat.Abstracoes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EscudoNarrador.Dominio.Abstracoes.Repositorios
{
    public interface IRepositorio<TEntidade> where TEntidade : class, new()
    {
        Task<TEntidade> AdicionarAsync(TEntidade entidade);
        Task<TEntidade> AtualizarAsync(TEntidade entidade);
        Task DeletarAsync(string chaveParticao, string chaveLinha);
        Task<TEntidade> ObterAsync(string chaveParticao, string chaveLinha);
        IEnumerable<TEntidade> ObterTodos<T>(string chaveParticao, Expression<Func<T, bool>> predicado);
        IEnumerable<TEntidade> ObterTodos<T>(string chaveParticao, IFiltro<T> filtro);
    }
}
