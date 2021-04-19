using EscudoNarrador.Repositorio.Configuracoes;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using Nebularium.Tiamat.Abstracoes;

namespace EscudoNarrador.Repositorio.Abstracoes
{
    public interface ICosmosTableContexto : IContexto
    {
        IOptions<ConexaoOpcoes> ObterConfiguracao { get; }
        CloudStorageAccount OberContaArmazenamento { get; }
        CloudTable ObterTabela<T>();
        CloudTable ObterTabela(string nomeTabela);
    }
}
