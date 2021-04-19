using EscudoNarrador.Repositorio.Abstracoes;
using EscudoNarrador.Repositorio.Atributos;
using EscudoNarrador.Repositorio.Configuracoes;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nebularium.Tarrasque.Extensoes;
using System;

namespace EscudoNarrador.Repositorio
{
    public class CosmosTableContextoBase : ICosmosTableContexto
    {
        const string ERRO_MSG = "Informação de conta de armazenamento informada inválida. Por favor confirnme se o AccountName e o AccountKey são válidos no arquivo app.config - então restart a aplicação.";
        private readonly IOptions<ConexaoOpcoes> configuracoes;
        private readonly CloudStorageAccount contaArmazenamento;
        private readonly ILogger<CosmosTableContextoBase> log;

        public IOptions<ConexaoOpcoes> ObterConfiguracao => configuracoes;
        public CloudStorageAccount OberContaArmazenamento => contaArmazenamento;

        public CosmosTableContextoBase(IOptions<ConexaoOpcoes> configuracao, ILogger<CosmosTableContextoBase> log)
        {
            this.configuracoes = configuracao;
            contaArmazenamento = CriaContaArmazenamentoDaConnectionString(configuracao.Value.ConnectionString);
        }
        private CloudStorageAccount CriaContaArmazenamentoDaConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is ArgumentException)
                    log.LogError(ERRO_MSG);
                throw;
            }

            return storageAccount;
        }

        public CloudTable ObterTabela<T>()
        {
            var nome = typeof(T).ObterAnotacao<NomeAttribute>()?.Nome;
            CloudTable table = ObterTabela(nome.LimpoNuloBranco() ? typeof(T).Name : nome);
            return table;
        }

        public CloudTable ObterTabela(string tableName)
        {
            CloudTableClient tableClient = contaArmazenamento.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);
            return table;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
