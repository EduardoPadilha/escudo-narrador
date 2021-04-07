using EscudoNarrador.Repositorio.Configuracoes;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using System;

namespace EscudoNarrador.Repositorio
{
    public class CosmosContexto
    {
        const string ERRO_MSG = "Informação de conta de armazenamento informada inválida. Por favor confirnme se o AccountName e o AccountKey são válidos no arquivo app.config - então restart a aplicação.";
        private readonly IOptions<ConexaoOpcoes> configuracao;
        private readonly CloudStorageAccount contaArmazenamento;
        public CosmosContexto(IOptions<ConexaoOpcoes> configuracao)
        {
            this.configuracao = configuracao;
            contaArmazenamento = CriaContaArmazenamentoDaConnectionString(configuracao.Value.ConnectionString);
        }
        public static CloudStorageAccount CriaContaArmazenamentoDaConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine(ERRO_MSG);
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine(ERRO_MSG);
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        public CloudTable ObterTabela(string tableName)
        {
            CloudTableClient tableClient = contaArmazenamento.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);
            return table;
        }
    }
}
