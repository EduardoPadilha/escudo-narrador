using EscudoNarrador.Entidade;
using System.Collections.Generic;

namespace EscudoNarrador.Dominio.Abstracoes.Repositorios
{
    public interface ISistemaRepositorio : IRepositorio<Termo>
    {
        IEnumerable<Termo> ObterTodos(string chaveParticao, string nome, string[] tags);
    }
}
