using Microsoft.Azure.Cosmos.Table;

namespace EscudoNarrador.Repositorio.Abstracoes
{
    public abstract class MapeamentoAbstrato<TEntidade> : TableEntity
        where TEntidade : class, new()
    {
        protected MapeamentoAbstrato() { }
    }
}
