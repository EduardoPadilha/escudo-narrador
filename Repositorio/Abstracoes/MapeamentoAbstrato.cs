using Microsoft.Azure.Cosmos.Table;

namespace EscudoNarrador.Repositorio.Abstracoes
{
    public abstract class MapeamentoAbstrato<T> : TableEntity
    {
        public MapeamentoAbstrato(T entidade)
        {
        }
        public MapeamentoAbstrato(string chaveParticao, string chaveLinha)
        {
            PartitionKey = chaveParticao;
            RowKey = chaveLinha;
        }
        public abstract T ParaEntidade();
    }
}
