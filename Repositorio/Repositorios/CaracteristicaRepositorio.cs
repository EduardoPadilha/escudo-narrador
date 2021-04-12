using EscudoNarrador.Repositorio.Excecoes;
using EscudoNarrador.Repositorio.Mapeamento;
using EscudoNarrador.Shared.Abstracoes.Repositorios;
using EscudoNarrador.Shared.Entidades;
using EscudoNarrador.Shared.Enums;
using EscudoNarrador.Shared.Extensoes;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Nebularium.Tarrasque.Extensoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscudoNarrador.Repositorio.Repositorios
{
    public class CaracteristicaRepositorio : ICaracteristicaRepositorio
    {
        private readonly CosmosContexto contexto;
        private readonly CloudTable tabela;
        private readonly ILogger<CaracteristicaRepositorio> log;
        public CaracteristicaRepositorio(CosmosContexto contexto, ILogger<CaracteristicaRepositorio> log)
        {
            this.contexto = contexto;
            this.log = log;
            this.tabela = contexto.ObterTabela("caracteristicas");
        }

        public async Task<Caracteristica> AdicionarAsync(Caracteristica entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException("entidade");

            try
            {
                var operacaoAddOuMerge = TableOperation.InsertOrMerge(new CaracteristicaMapeamento(entidade));

                var resultado = await tabela.ExecuteAsync(operacaoAddOuMerge);
                var insertedCustomer = resultado.Result as CaracteristicaMapeamento;

                if (resultado.RequestCharge.HasValue)
                    log.LogInformation("Request Charge of InsertOrMerge Operation: " + resultado.RequestCharge);

                return insertedCustomer.ParaEntidade();
            }
            catch (StorageException e)
            {
                log.LogError(e, "Erro ao adicionar uma caracateristica");
                throw;
            }
        }

        public async Task<Caracteristica> ObterAsync(string nome, TipoSistema sistema)
        {
            try
            {
                var operacoBusca = TableOperation.Retrieve<CaracteristicaMapeamento>(sistema.ToString(), nome);
                var result = await tabela.ExecuteAsync(operacoBusca);
                var caracteristicaBd = result.Result as CaracteristicaMapeamento;
                if (caracteristicaBd == null)
                    throw new RecursoNaoEncontradoException();

                log.LogInformation(JsonConvert.SerializeObject(caracteristicaBd));

                if (result.RequestCharge.HasValue)
                {
                    log.LogInformation("Request Charge of Retrieve Operation: " + result.RequestCharge);
                }

                return caracteristicaBd?.ParaEntidade();
            }
            catch (StorageException e)
            {
                log.LogError(e, $"Erro ao recuperar a caracateristica {nome}");
                throw;
            }
        }

        public List<Caracteristica> ObterTodos(string nome, TipoSistema sistema, string[] tags)
        {
            try
            {
                var query = tabela.CreateQuery<CaracteristicaMapeamento>()
                    .Where(c => c.PartitionKey == sistema.ToString());

                var caracteristicasBd = query.ToList();
                if (caracteristicasBd == null)
                    throw new RecursoNaoEncontradoException();

                var tagsHigienizadas = tags.Select(c => c.HigienizaString());

                IEnumerable<CaracteristicaMapeamento> consulta = caracteristicasBd;
                if (!nome.LimpoNuloBranco())
                    consulta = consulta.Where(c => c.RowKey.Contains(nome));

                if (tags.AnySafe())
                    consulta = consulta.Where(c => tagsHigienizadas.All(tag => c.TagsHigienizadas.Split(";").Contains(tag)));

                var caracteristicas = consulta.ToList().ConvertAll(c => c.ParaEntidade());

                return caracteristicas;
            }
            catch (StorageException e)
            {
                log.LogError(e, $"Erro ao recuperar a caracateristicas");
                throw;
            }
        }
    }
}
