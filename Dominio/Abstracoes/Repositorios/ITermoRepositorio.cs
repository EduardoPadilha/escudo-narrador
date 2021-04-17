using EscudoNarrador.Entidade;
using System.Collections.Generic;

namespace EscudoNarrador.Dominio.Abstracoes.Repositorios
{
    public interface ITermoRepositorio : IRepositorio<Termo>
    {
        IEnumerable<Termo> ObterTodos(string chaveParticao, string nome, string[] tags);
    }
}
