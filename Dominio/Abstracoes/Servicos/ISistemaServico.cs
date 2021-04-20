using EscudoNarrador.Entidade;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscudoNarrador.Dominio.Abstracoes.Servicos
{
    public interface ISistemaServico
    {
        IEnumerable<Termo> ObterTodos();
        IEnumerable<Termo> ObterTodos(string nome, string versao);
        Task<Sistema> ObterAsync(string nome, string versao);
        Task<Sistema> AdicionarAsync(Sistema termo);
        Task<Sistema> AtualizarAsync(Sistema termo);
        Task DeletarAsync(string nome, string versao);
    }
}
